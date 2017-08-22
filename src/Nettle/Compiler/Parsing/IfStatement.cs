namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Represents an 'if' statement code block
    /// </summary>
    internal class IfStatement : NestableCodeBlock
    {
        /// <summary>
        /// Gets or sets the statements condition name
        /// </summary>
        public string ConditionName { get; set; }

        /// <summary>
        /// Gets or sets the conditions value type
        /// </summary>
        public NettleValueType ConditionType { get; set; }
    }
}
