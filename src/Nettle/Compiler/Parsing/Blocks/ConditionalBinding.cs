namespace Nettle.Compiler.Parsing.Blocks
{
    using Nettle.Compiler.Parsing.Conditions;

    /// <summary>
    /// Represents a conditional binding code block
    /// </summary>
    internal class ConditionalBinding : CodeBlock
    {
        /// <summary>
        /// Gets or sets the conditions expression
        /// </summary>
        public BooleanExpression ConditionExpression { get; set; }
        
        /// <summary>
        /// Gets or sets the signature of the true value
        /// </summary>
        public string TrueValueSignature { get; set; }

        /// <summary>
        /// Gets or sets the true value
        /// </summary>
        public object TrueValue { get; set; }

        /// <summary>
        /// Gets or sets the true value type
        /// </summary>
        public NettleValueType TrueValueType { get; set; }

        /// <summary>
        /// Gets or sets the signature of the false value
        /// </summary>
        public string FalseValueSignature { get; set; }

        /// <summary>
        /// Gets or sets the false value
        /// </summary>
        public object FalseValue { get; set; }

        /// <summary>
        /// Gets or sets the false value type
        /// </summary>
        public NettleValueType FalseValueType { get; set; }
    }
}
