namespace Nettle.Parsing
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
        /// Gets or sets a flag indicating if the collection is a model binding
        /// </summary>
        public bool IsModelBinding { get; set; }
    }
}
