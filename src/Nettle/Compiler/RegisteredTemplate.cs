namespace Nettle.Compiler
{
    using System;

    /// <summary>
    /// Represents a registered template
    /// </summary>
    public sealed class RegisteredTemplate
    {
        /// <summary>
        /// Constructs the registered template with dependencies
        /// </summary>
        /// <param name="name">The template name</param>
        /// <param name="parsedTemplate">The parsed template</param>
        /// <param name="compiledTemplate">The compiled template</param>
        internal RegisteredTemplate
            (
                string name,
                Template parsedTemplate,
                Func<object, string> compiledTemplate
            )
        {
            Validate.IsNotEmpty(name);
            Validate.IsNotNull(parsedTemplate);
            Validate.IsNotNull(compiledTemplate);

            if (false == name.IsAlphaNumeric())
            {
                throw new ArgumentException
                (
                    "The name '{0}' is invalid for a template. " +
                    "Names must be alphanumeric.".With
                    (
                        name
                    )
                );
            }

            this.Name = name;
            this.ParsedTemplate = parsedTemplate;
            this.CompiledTemplate = compiledTemplate;
        }

        /// <summary>
        /// Gets the registered templates name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the parsed template
        /// </summary>
        internal Template ParsedTemplate
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the compiled template
        /// </summary>
        public Func<object, string> CompiledTemplate
        {
            get;
            private set;
        }
    }
}
