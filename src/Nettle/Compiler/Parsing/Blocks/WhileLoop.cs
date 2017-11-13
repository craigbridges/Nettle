namespace Nettle.Compiler.Parsing.Blocks
{
    using Nettle.Compiler.Parsing.Conditions;

    /// <summary>
    /// Represents a 'while' loop code block
    /// </summary>
    internal class WhileLoop : NestableCodeBlock
    {
        /// <summary>
        /// Gets or sets the loops expression
        /// </summary>
        public BooleanExpression ConditionExpression { get; set; }
    }
}
