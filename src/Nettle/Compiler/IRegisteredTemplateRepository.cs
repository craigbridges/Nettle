namespace Nettle.Compiler
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a contract for a repository that manages registered templates
    /// </summary>
    internal interface IRegisteredTemplateRepository
    {
        /// <summary>
        /// Adds a registered template to the repository
        /// </summary>
        /// <param name="template">The registered template</param>
        void Add(RegisteredTemplate template);

        /// <summary>
        /// Gets a registered template from the repository
        /// </summary>
        /// <param name="name">The registered template name</param>
        /// <returns>The matching registered template</returns>
        RegisteredTemplate Get(string name);

        /// <summary>
        /// Gets a collection of all registered templates in the repository
        /// </summary>
        /// <returns>A collection of matching templates</returns>
        IEnumerable<RegisteredTemplate> GetAll();

        /// <summary>
        /// Removes a registered template from the repository
        /// </summary>
        /// <param name="name">The registered template name</param>
        void Remove(string name);
    }
}
