namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a parsed template
    /// </summary>
    internal class Template
    {
        /// <summary>
        /// Gets the parent template
        /// </summary>
        public Template Parent { get; protected set; }

        /// <summary>
        /// Gets the templates raw text
        /// </summary>
        public string RawText { get; protected set; }

        /// <summary>
        /// Gets an array of the model bindings
        /// </summary>
        public ModelBinding[] Bindings { get; protected set; }

        /// <summary>
        /// Gets an array of the function calls
        /// </summary>
        public FunctionCall[] Functions { get; protected set; }

        /// <summary>
        /// Gets an array of the variable assignments
        /// </summary>
        public VariableDeclaration[] Variables { get; protected set; }

        /// <summary>
        /// Gets an array of the for each loops
        /// </summary>
        public ForEachLoop[] Iterators { get; protected set; }

        /// <summary>
        /// Gets an array of the if statements
        /// </summary>
        public IfStatement[] Conditions { get; protected set; }
    }
}
