namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a variable declaration code block
    /// </summary>
    internal class VariableDeclaration : CodeBlock
    {
        /// <summary>
        /// Gets or sets the variable name
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// Gets or sets the signature of the assigned value
        /// </summary>
        public string AssignedValueSignature { get; set; }

        /// <summary>
        /// Gets or sets the variables assignment type
        /// </summary>
        public VariableAssignmentType AssignmentType { get; set; }

        /// <summary>
        /// Gets or sets the function call block
        /// </summary>
        public FunctionCall FunctionCall { get; set; }
    }
}
