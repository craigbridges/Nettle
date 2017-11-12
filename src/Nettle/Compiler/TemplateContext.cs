namespace Nettle.Compiler
{
    using Nettle.Compiler.Parsing;
    using System;
    using System.Collections;
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
        /// <param name="model">The model</param>
        /// <param name="flags">The template flags</param>
        internal TemplateContext
            (
                object model,
                params TemplateFlag[] flags
            )
        {
            this.Model = model;
            this.PartialCallStack = new List<string>();
            this.PropertyValues = new Dictionary<string, object>();
            this.Variables = new Dictionary<string, object>();

            if (flags == null)
            {
                this.Flags = new TemplateFlag[] { };
            }
            else
            {
                this.Flags = flags;
            }

            PopulatePropertyValues(model);
        }

        /// <summary>
        /// Constructs the template context with a model and parent
        /// </summary>
        /// <param name="parent">The parent template context</param>
        /// <param name="model">The model</param>
        /// <param name="flags">The template flags</param>
        private TemplateContext
            (
                TemplateContext parent,
                object model,
                params TemplateFlag[] flags
            )

            : this(model, flags)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// Gets the parent template context
        /// </summary>
        private TemplateContext Parent { get; set; }

        /// <summary>
        /// Determines if this is the root template context
        /// </summary>
        /// <returns>True, if it's the root context; otherwise false</returns>
        public bool IsRoot()
        {
            return this.Parent == null;
        }

        /// <summary>
        /// Gets the templates model
        /// </summary>
        public object Model { get; private set; }

        /// <summary>
        /// Gets an array of template flags
        /// </summary>
        public TemplateFlag[] Flags { get; private set; }

        /// <summary>
        /// Determines if a specific template flag has been set
        /// </summary>
        /// <param name="flag">The flag</param>
        /// <returns>True, if it has been set; otherwise false</returns>
        public bool IsFlagSet
            (
                TemplateFlag flag
            )
        {
            return this.Flags.Contains
            (
                flag
            );
        }

        /// <summary>
        /// Gets a call stack of partials that have been rendered
        /// </summary>
        internal List<string> PartialCallStack { get; private set; }

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
                var indexerInfo = new IndexerInfo
                (
                    propertyPath
                );

                if (indexerInfo.HasIndexer)
                {
                    propertyPath = indexerInfo.PathWithoutIndexer;
                }

                var nameFound = this.PropertyValues.ContainsKey
                (
                    propertyPath
                );

                if (false == nameFound)
                {
                    var allowImplicit = IsFlagSet
                    (
                        TemplateFlag.AllowImplicitBindings
                    );

                    if (allowImplicit)
                    {
                        return null;
                    }
                    else
                    {
                        var message = "No property could be found with the name '{0}'.";

                        throw new NettleRenderException
                        (
                            message.With(propertyPath)
                        );
                    }
                }

                if (indexerInfo.HasIndexer)
                {
                    var collection = this.PropertyValues
                    [
                        propertyPath
                    ];

                    return ResolveIndexedBinding
                    (
                        propertyPath,
                        collection,
                        indexerInfo
                    );
                }
                else
                {
                    return this.PropertyValues
                    [
                        propertyPath
                    ];
                }
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
                var message = "Property {0} cannot be resolved because the model is null.";

                throw new NettleRenderException
                (
                    message.With
                    (
                        propertyPath
                    )
                );
            }

            var indexerInfo = new IndexerInfo
            (
                propertyPath
            );

            if (indexerInfo.HasIndexer)
            {
                propertyPath = indexerInfo.PathWithoutIndexer;
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
                    var message = "The path '{0}' contains a null reference at '{1}'.";

                    throw new NettleRenderException
                    (
                        message.With
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
                    var allowImplicit = IsFlagSet
                    (
                        TemplateFlag.AllowImplicitBindings
                    );

                    if (allowImplicit)
                    {
                        return null;
                    }
                    else
                    {
                        var message = "The path '{0}' does not contain a property named '{1}'.";

                        throw new NettleRenderException
                        (
                            message.With
                            (
                                propertyPath,
                                nextName
                            )
                        );
                    }
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
            
            if (indexerInfo.HasIndexer)
            {
                return ResolveIndexedBinding
                (
                    propertyPath,
                    currentValue,
                    indexerInfo
                );
            }
            else
            {
                return currentValue;
            }
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
                var message = "A variable with the name '{0}' has already been defined.";

                throw new InvalidOperationException
                (
                    message.With(name)
                );
            }

            this.Variables[name] = value;
        }

        /// <summary>
        /// Reassigns the value of a variable for the template context
        /// </summary>
        /// <param name="name">The variable name</param>
        /// <param name="value">The variables value</param>
        internal void ReassignVariable
            (
                string name,
                object value
            )
        {
            Validate.IsNotEmpty(name);

            if (false == this.Variables.ContainsKey(name))
            {
                throw new InvalidOperationException
                (
                    "No variable was defined with the name '{0}'.".With
                    (
                        name
                    )
                );
            }

            var strictReassign = IsFlagSet
            (
                TemplateFlag.EnforceStrictReassign
            );

            if (strictReassign && value != null)
            {
                var currentValue = this.Variables[name];

                if (currentValue != null)
                {
                    var oldValueType = currentValue.GetType();
                    var newValueType = value.GetType();

                    var isAssignable = oldValueType.IsAssignableFrom
                    (
                        newValueType
                    );

                    if (false == isAssignable)
                    {
                        throw new InvalidOperationException
                        (
                            "The type {0} cannot be assigned to type {1}.".With
                            (
                                newValueType.Name,
                                oldValueType.Name
                            )
                        );
                    }
                }
            }

            this.Variables[name] = value;

            var parent = this.Parent;

            if (parent != null)
            {
                var isInherited = parent.Variables.ContainsKey
                (
                    name
                );

                parent.ReassignVariable
                (
                    name,
                    value
                );
            }
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
                var indexerInfo = new IndexerInfo
                (
                    variablePath
                );

                if (indexerInfo.HasIndexer)
                {
                    variablePath = indexerInfo.PathWithoutIndexer;
                }

                var variableFound = this.Variables.ContainsKey
                (
                    variablePath
                );

                if (false == variableFound)
                {
                    var allowImplicit = IsFlagSet
                    (
                        TemplateFlag.AllowImplicitBindings
                    );

                    if (allowImplicit)
                    {
                        return null;
                    }
                    else
                    {
                        var message = "No variable has been defined with the name '{0}'.";

                        throw new NettleRenderException
                        (
                            message.With(variablePath)
                        );
                    }
                }

                if (indexerInfo.HasIndexer)
                {
                    var collection = this.Variables
                    [
                        variablePath
                    ];

                    return ResolveIndexedBinding
                    (
                        variablePath,
                        collection,
                        indexerInfo
                    );
                }
                else
                {
                    return this.Variables
                    [
                        variablePath
                    ];
                }
            }
        }

        /// <summary>
        /// Resolves an indexed binding value
        /// </summary>
        /// <param name="bindingPath">The binding path</param>
        /// <param name="collection">The collection</param>
        /// <param name="indexer">The indexer information</param>
        /// <returns>The value found at the specified index</returns>
        private object ResolveIndexedBinding
            (
                string bindingPath,
                object collection,
                IndexerInfo indexer
            )
        {
            if (collection == null)
            {
                throw new InvalidOperationException
                (
                    "The value for '{0}' is null.".With
                    (
                        bindingPath
                    )
                );
            }

            if (false == collection.GetType().IsEnumerable())
            {
                throw new InvalidOperationException
                (
                    "'{0}' is not a valid collection.".With
                    (
                        bindingPath
                    )
                );
            }

            var index = ResolverIndexerValue(indexer);

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException
                (
                    "The index for '{0}' must be zero or greater.".With
                    (
                        bindingPath
                    )
                );
            }

            var counter = default(int);

            foreach (var item in collection as IEnumerable)
            {
                if (counter == index)
                {
                    return item;
                }

                counter++;
            }

            throw new IndexOutOfRangeException
            (
                "The index {0} for '{1}' is out of range.".With
                (
                    index,
                    bindingPath
                )
            );
        }

        /// <summary>
        /// Resolves an indexer to a numeric value
        /// </summary>
        /// <param name="indexer">The indexer</param>
        /// <returns>The resolved value</returns>
        private int ResolverIndexerValue
            (
                IndexerInfo indexer
            )
        {
            switch (indexer.IndexerValueType)
            {
                case NettleValueType.Number:
                {
                    return indexer.ResolvedIndex;
                }
                case NettleValueType.Variable:
                {
                    var value = ResolveVariableValue
                    (
                        indexer.IndexerSignature
                    );
                    
                    if (value == null)
                    {
                        throw new NullReferenceException
                        (
                            "The indexer must resolve to a numeric value."
                        );
                    }

                    if (value.GetType().IsNumeric())
                    {
                        return Convert.ToInt32(value);
                    }
                    else
                    {
                        throw new InvalidCastException
                        (
                            "{0} is not a valid indexer type.".With
                            (
                                value.GetType().Name
                            )
                        );
                    }
                }
                default:
                {
                    return -1;
                }
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
                this,
                model,
                this.Flags
            );

            // Copy the partial call stack across
            foreach (var partial in this.PartialCallStack)
            {
                context.PartialCallStack.Add
                (
                    partial
                );
            }

            var disableInheritance = IsFlagSet
            (
                TemplateFlag.DisableModelInheritance
            );

            if (false == disableInheritance)
            {
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
            }

            return context;
        }
    }
}
