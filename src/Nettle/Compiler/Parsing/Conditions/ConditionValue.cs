namespace Nettle.Compiler.Parsing.Conditions
{
    /// <summary>
    /// Represents a boolean condition value
    /// </summary>
    internal class ConditionValue
    {
        /// <summary>
        /// Constructs the condition with the details
        /// </summary>
        /// <param name="signature">The conditions signature</param>
        /// <param name="valueType">The value type</param>
        /// <param name="value">The parsed value</param>
        public ConditionValue
            (
                string signature,
                NettleValueType valueType,
                object value
            )
        {
            Validate.IsNotEmpty(signature);

            this.Signature = signature;
            this.ValueType = valueType;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the statements conditions signature
        /// </summary>
        public string Signature { get; private set; }

        /// <summary>
        /// Gets or sets the conditions value type
        /// </summary>
        public NettleValueType ValueType { get; private set; }

        /// <summary>
        /// Gets or sets the conditions parsed value
        /// </summary>
        public object Value { get; private set; }
    }
}
