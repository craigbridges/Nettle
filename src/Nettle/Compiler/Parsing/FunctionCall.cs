namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Represents a model binding code block
    /// </summary>
    internal class FunctionCall : CodeBlock
    {
        /// <summary>
        /// Gets or sets the name of the function
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Gets or sets an array of the parameters supplied
        /// </summary>
        public FunctionCallParameter[] ParameterValues { get; set; }
    }
}
