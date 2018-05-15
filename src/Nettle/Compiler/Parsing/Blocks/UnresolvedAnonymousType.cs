namespace Nettle.Compiler.Parsing.Blocks
{
    /// <summary>
    /// Represents an unresolved anonymous type code block
    /// </summary>
    internal class UnresolvedAnonymousType : CodeBlock
    {
        /// <summary>
        /// Gets an array of properties contained in the type
        /// </summary>
        public UnresolvedAnonymousTypeProperty[] Properties { get; set; }
    }
}
