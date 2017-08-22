namespace Nettle.Compiler
{
    using System;
    using System.Collections.Generic;

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
                // TODO: read all properties
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
    }
}
