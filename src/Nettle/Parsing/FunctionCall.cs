namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a model binding code block
    /// </summary>
    internal class FunctionCall : CodeBlock
    {
        /// <summary>
        /// Gets the name of the function
        /// </summary>
        public string FunctionName { get; protected set; }

        /// <summary>
        /// Gets an array of the parameters supplied
        /// </summary>
        public FunctionCallParameter[] ParameterValues { get; protected set; }
    }
}
