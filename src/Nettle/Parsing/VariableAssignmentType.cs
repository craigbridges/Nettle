namespace Nettle.Parsing
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a collection of variable assignment types
    /// </summary>
    internal enum VariableAssignmentType
    {
        [Description("Model Binding")]
        ModelBinding = 0,

        [Description("Function")]
        Function = 1
    }
}
