namespace Nettle.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the default implementation of a registered template repository
    /// </summary>
    public sealed class RegisteredTemplateRepository : IRegisteredTemplateRepository
    {
        private Dictionary<string, RegisteredTemplate> _templates;

        public RegisteredTemplateRepository()
        {
            _templates = new Dictionary<string, RegisteredTemplate>();
        }

        /// <summary>
        /// Adds a registered template to the repository
        /// </summary>
        /// <param name="template">The registered template</param>
        public void Add
            (
                RegisteredTemplate template
            )
        {
            Validate.IsNotNull(template);

            var name = template.Name;
            var found = _templates.ContainsKey(name);

            if (found)
            {
                throw new InvalidOperationException
                (
                    "A template with the name '{0}' has already been registered.".With
                    (
                        name
                    )
                );
            }

            _templates.Add(name, template);
        }

        /// <summary>
        /// Gets a registered template from the repository
        /// </summary>
        /// <param name="name">The registered template name</param>
        /// <returns>The matching registered template</returns>
        public RegisteredTemplate Get
            (
                string name
            )
        {
            Validate.IsNotEmpty(name);

            if (false == _templates.ContainsKey(name))
            {
                throw new KeyNotFoundException
                (
                    "No template has been registered with the name '{0}'.".With
                    (
                        name
                    )
                );
            }

            return _templates[name];
        }

        /// <summary>
        /// Gets a collection of all registered templates in the repository
        /// </summary>
        /// <returns>A collection of matching templates</returns>
        public IEnumerable<RegisteredTemplate> GetAll()
        {
            return _templates.Select
            (
                item => item.Value
            );
        }

        /// <summary>
        /// Removes a registered template from the repository
        /// </summary>
        /// <param name="name">The registered template name</param>
        public void Remove
            (
                string name
            )
        {
            Validate.IsNotEmpty(name);

            if (false == _templates.ContainsKey(name))
            {
                throw new KeyNotFoundException
                (
                    "No template has been registered with the name '{0}'.".With
                    (
                        name
                    )
                );
            }

            _templates.Remove(name);
        }
    }
}
