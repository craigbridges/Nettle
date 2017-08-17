namespace Nettle.Parsing
{
    /// <summary>
    /// Represents an 'if' statement code block
    /// </summary>
    internal class IfStatement : CodeBlock
    {
        /// <summary>
        /// Gets the statements condition name
        /// </summary>
        public string ConditionName { get; protected set; }

        /// <summary>
        /// Gets the statements body
        /// </summary>
        public Template Body { get; protected set; }
    }
}
