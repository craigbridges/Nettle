namespace Nettle.Parsing
{
    /// <summary>
    /// Represents a variable declaration code block
    /// </summary>
    internal class VariableDeclaration : CodeBlock
    {
        /// <summary>
        /// Gets the variable name
        /// </summary>
        public string VariableName { get; protected set; }

        /// <summary>
        /// Gets the signature of the assigned value
        /// </summary>
        public string AssignedValueSignature { get; protected set; }

        /// <summary>
        /// Gets the variables assignment type
        /// </summary>
        public VariableAssignmentType AssignmentType { get; protected set; }

        /// <summary>
        /// Gets the function call block
        /// </summary>
        public FunctionCall FunctionCall { get; protected set; }
    }
}
