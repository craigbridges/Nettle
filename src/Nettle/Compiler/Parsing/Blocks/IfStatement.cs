namespace Nettle.Compiler.Parsing.Blocks
{
    using Nettle.Compiler.Parsing.Conditions;

    /// <summary>
    /// Represents an 'if' statement code block
    /// </summary>
    internal class IfStatement : NestableCodeBlock
    {
        /// <summary>
        /// Gets or sets the conditions expression
        /// </summary>
        public BooleanExpression ConditionExpression { get; set; }
    }
}
