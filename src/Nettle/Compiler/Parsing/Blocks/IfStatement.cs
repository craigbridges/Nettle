namespace Nettle.Compiler.Parsing.Blocks
{
    using Nettle.Compiler.Parsing.Conditions;

    /// <summary>
    /// Represents an 'if' statement code block
    /// </summary>
    internal class IfStatement : NestableCodeBlock
    {
        /// <summary>
        /// Gets or sets the statements conditions signature
        /// </summary>
        public string ConditionSignature { get; set; }

        /// <summary>
        /// Gets or sets the conditions value type
        /// </summary>
        public NettleValueType ConditionType { get; set; }

        /// <summary>
        /// Gets or sets the statements conditions value
        /// </summary>
        public object ConditionValue { get; set; }

        /// <summary>
        /// Gets or sets the conditions expression
        /// </summary>
        public BooleanExpression ConditionExpression { get; set; }
    }
}
