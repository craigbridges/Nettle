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
        /// <param name="name">The name</param>
        /// <param name="template">The compiled template</param>
        public RegisteredTemplate
            (
                string name,
                Func<object, string> template
            )
        {
            Validate.IsNotEmpty(name);
            Validate.IsNotNull(template);

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
            this.Template = template;
        }

        /// <summary>
        /// Gets the registered templates name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the compiled template
        /// </summary>
        public Func<object, string> Template { get; private set; }
    }
}
