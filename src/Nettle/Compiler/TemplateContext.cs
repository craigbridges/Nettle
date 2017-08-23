namespace Nettle.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents a Nettle template context
    /// </summary>
    public class TemplateContext
    {
        /// <summary>
        /// Constructs the template context with a model
        /// </summary>
        internal TemplateContext
            (
                object model
            )
        {
            this.Model = model;
            this.PropertyValues = new Dictionary<string, object>();
            this.Variables = new Dictionary<string, object>();

            if (model != null)
            {
                var properties = model.GetType().GetProperties
                (
                    BindingFlags.Public | BindingFlags.Instance
                );

                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue
                    (
                        model
                    );

                    this.PropertyValues.Add
                    (
                        property.Name,
                        propertyValue
                    );
                }
            }
        }

        /// <summary>
        /// Gets the templates model
        /// </summary>
        public object Model { get; private set; }

        /// <summary>
        /// Gets the contexts property values
        /// </summary>
        public Dictionary<string, object> PropertyValues
        {
            get;
            private set;
        }

        /// <summary>
        /// Resolves a property value from the property path specified
        /// </summary>
        /// <param name="propertyPath">The property path</param>
        /// <returns>The property value found</returns>
        public object ResolvePropertyValue
            (
                string propertyPath
            )
        {
            Validate.IsNotEmpty(propertyPath);

            var isNested = TemplateContext.IsNestedProperty
            (
                propertyPath
            );

            // Try to resolve a nested variable if it looks like one
            if (isNested)
            {
                var segments = propertyPath.Split('.');
                var nextName = segments[0];

                var containsEmptyParts = segments.Any
                (
                    part => String.IsNullOrWhiteSpace(part)
                );

                // Quickly validate the nested name syntax
                if (containsEmptyParts)
                {
                    throw new NettleRenderException
                    (
                        "The property path '{0}' is invalid.".With
                        (
                            propertyPath
                        )
                    );
                }

                var currentValue = ResolvePropertyValue
                (
                    nextName
                );

                // Try to resolve each segment one at a time until the end is reached
                for (var i = 1; i < segments.Length; i++)
                {
                    if (currentValue == null)
                    {
                        throw new NettleRenderException
                        (
                            "The path '{0}' contains a null reference at '{1}'.".With
                            (
                                propertyPath,
                                nextName
                            )
                        );
                    }

                    nextName = segments[i];

                    var currentType = currentValue.GetType();
                    var propertyFound = currentType.HasProperty(nextName);

                    if (false == propertyFound)
                    {
                        throw new NettleRenderException
                        (
                            "The path '{0}' does not contain a property named '{1}'.".With
                            (
                                propertyPath,
                                nextName
                            )
                        );
                    }

                    var nextProperty = currentType.GetProperty
                    (
                        nextName
                    );

                    currentValue = nextProperty.GetValue
                    (
                        currentValue
                    );
                }

                return currentValue;
            }
            else
            {
                var nameFound = this.PropertyValues.ContainsKey
                (
                    propertyPath
                );

                if (false == nameFound)
                {
                    throw new NettleRenderException
                    (
                        "No property could be found with the name '{0}'.".With
                        (
                            propertyPath
                        )
                    );
                }

                return this.PropertyValues[propertyPath];
            }
        }

        /// <summary>
        /// Determines if a property path represents a nested property
        /// </summary>
        /// <param name="propertyPath">The variable name</param>
        /// <returns>True, if the variable name is nested; otherwise false</returns>
        /// <remarks>
        /// A nested variable is one that is separated by dots (".")
        /// </remarks>
        public static bool IsNestedProperty
            (
                string propertyPath
            )
        {
            if (String.IsNullOrEmpty(propertyPath))
            {
                return false;
            }
            else
            {
                var segments = propertyPath.Split('.').Where
                (
                    part => false == String.IsNullOrEmpty(part)
                );

                return
                (
                    segments.Count() > 1
                );
            }
        }

        /// <summary>
        /// Gets the contexts variables
        /// </summary>
        public Dictionary<string, object> Variables
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines a variable for the template context
        /// </summary>
        /// <param name="name">The variable name</param>
        /// <param name="value">The variables value</param>
        internal void DefineVariable
            (
                string name,
                object value
            )
        {
            Validate.IsNotEmpty(name);
            
            if (this.Variables.ContainsKey(name))
            {
                throw new InvalidOperationException
                (
                    "A variable with the name '{0}' has already been defined.".With
                    (
                        name
                    )
                );
            }

            this.Variables[name] = value;
        }

        /// <summary>
        /// Creates a nested context that inherits from the current context
        /// </summary>
        /// <param name="model">The new model data</param>
        /// <returns>The template context created</returns>
        internal TemplateContext CreateNestedContext
            (
                object model
            )
        {
            var context = new TemplateContext
            (
                model
            );

            // Migrate any properties that do not conflict with the new model
            foreach (var item in this.PropertyValues)
            {
                var propertyFound = context.PropertyValues.ContainsKey
                (
                    item.Key
                );

                if (false == propertyFound)
                {
                    context.PropertyValues.Add
                    (
                        item.Key,
                        item.Value
                    );
                }
            }

            // Migrate the variables across
            foreach (var variable in this.Variables)
            {
                context.Variables.Add
                (
                    variable.Key,
                    variable.Value
                );
            }

            return context;
        }
    }
}
