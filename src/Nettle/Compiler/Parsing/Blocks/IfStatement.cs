namespace Nettle.Compiler.Parsing.Blocks
{
    using Nettle.Compiler.Parsing.Conditions;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an 'if' statement code block
    /// </summary>
    internal class IfStatement : NestableCodeBlock
    {
        /// <summary>
        /// Gets or sets the conditions expression
        /// </summary>
        public BooleanExpression ConditionExpression { get; set; }

        /// <summary>
        /// Gets or sets a list of else if conditions
        /// </summary>
        public List<ElseIfStatement> ElseIfConditions { get; set; }

        /// <summary>
        /// Gets or sets the else content
        /// </summary>
        public NestableCodeBlock ElseContent { get; set; }
    }
}
