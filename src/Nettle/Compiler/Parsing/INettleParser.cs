namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Defines a contract for a Nettle parser
    /// </summary>
    internal interface INettleParser
    {
        /// <summary>
        /// Parses the content specified into a template
        /// </summary>
        /// <param name="content">The content to parse</param>
        /// <returns>The template</returns>
        Template Parse
        (
            string content
        );
    }
}
