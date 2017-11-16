namespace Nettle.Compiler.Parsing.Blocks
{
    using Nettle.Compiler.Parsing.Conditions;

    /// <summary>
    /// Represents an 'else if' statement code block
    /// </summary>
    internal class ElseIfStatement : NestableCodeBlock
    {
        /// <summary>
        /// Gets or sets the conditions expression
        /// </summary>
        public BooleanExpression ConditionExpression { get; set; }
    }
}
