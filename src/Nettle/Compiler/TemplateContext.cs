namespace Nettle.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents a Nettle template context
    /// </summary>
    public sealed class TemplateContext
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

            PopulatePropertyValues(model);
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
        /// Populates the property values by scanning the model
        /// </summary>
        /// <param name="model">The model data</param>
        private void PopulatePropertyValues
            (
                object model
            )
        {
            if (model != null)
            {
                var properties = model.GetType().GetProperties
                (
                    BindingFlags.Public | BindingFlags.Instance
                );

                foreach (var property in properties)
                {
                    var indexParamaters = property.GetIndexParameters();

                    if (indexParamaters == null || indexParamaters.Length == 0)
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

            var isModelRef = TemplateContext.IsModelReference
            (
                propertyPath
            );

            if (isModelRef)
            {
                return this.Model;
            }

            var isNested = TemplateContext.IsNested
            (
                propertyPath
            );

            // Try to resolve a nested property if it looks like one
            if (isNested)
            {
                var propertyName = ExtractNextSegment
                (
                    ref propertyPath
                );

                var firstValue = ResolvePropertyValue
                (
                    propertyName
                );

                var nestedValue = ResolveNestedPropertyValue
                (
                    firstValue,
                    propertyPath
                );

                return nestedValue;
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

                return this.PropertyValues
                [
                    propertyPath
                ];
            }
        }

        /// <summary>
        /// Resolves a nested property value from the property path specified
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="propertyPath">The property path</param>
        /// <returns>The property value found</returns>
        private object ResolveNestedPropertyValue
            (
                object model,
                string propertyPath
            )
        {
            Validate.IsNotEmpty(propertyPath);

            if (model == null)
            {
                throw new NettleRenderException
                (
                    "{0} cannot be resolved because the model is null.".With
                    (
                        propertyPath
                    )
                );
            }
            
            var segments = propertyPath.Split('.');
            var nextName = segments[0];
            var currentValue = model;

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

            // Try to resolve each segment one at a time until the end is reached
            for (var i = 0; i < segments.Length; i++)
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

        /// <summary>
        /// Determines if a property path is a model reference
        /// </summary>
        /// <param name="path">The property path</param>
        /// <returns>True, if the path is a model reference; otherwise false</returns>
        /// <remarks>
        /// A model reference indicates that the entire model should be 
        /// issued instead of a specific property value.
        /// </remarks>
        public static bool IsModelReference
            (
                string path
            )
        {
            if (String.IsNullOrEmpty(path))
            {
                return false;
            }
            else
            {
                return path.Trim().Equals(@"$");
            }
        }

        /// <summary>
        /// Determines if a property or variable path represents a nested property
        /// </summary>
        /// <param name="path">The property or variable path</param>
        /// <returns>True, if the path is nested; otherwise false</returns>
        /// <remarks>
        /// A nested path is one that is separated into segments by dots (".")
        /// </remarks>
        public static bool IsNested
            (
                string path
            )
        {
            if (String.IsNullOrEmpty(path))
            {
                return false;
            }
            else
            {
                var segments = path.Split('.').Where
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
        /// Extracts the next segment from a property or variable path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The matching segment</returns>
        internal static string ExtractNextSegment
            (
                ref string path
            )
        {
            var segments = path.Split('.');
            var nextSegment = segments[0];

            if (segments.Length == 1)
            {
                path = String.Empty;
            }
            else
            {
                path = path.Crop
                (
                    nextSegment.Length + 1
                );
            }

            return nextSegment;
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
        /// Resolves a variable value from the variable path specified
        /// </summary>
        /// <param name="variablePath">The variable path</param>
        /// <returns>The variable value found</returns>
        public object ResolveVariableValue
            (
                string variablePath
            )
        {
            Validate.IsNotEmpty(variablePath);

            var isNested = TemplateContext.IsNested
            (
                variablePath
            );

            if (isNested)
            {
                var variableName = ExtractNextSegment
                (
                    ref variablePath
                );

                var variableValue = ResolveVariableValue
                (
                    variableName
                );

                var nestedValue = ResolveNestedPropertyValue
                (
                    variableValue,
                    variablePath
                );

                return nestedValue;
            }
            else
            {
                var variableFound = this.Variables.ContainsKey
                (
                    variablePath
                );

                if (false == variableFound)
                {
                    throw new NettleRenderException
                    (
                        "No variable has been defined with the name '{0}'.".With
                        (
                            variablePath
                        )
                    );
                }

                return this.Variables
                [
                    variablePath
                ];
            }
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
