namespace Nettle.Compiler.Parsing.Conditions
{
    using System.ComponentModel;

    /// <summary>
    /// Defines condition value comparison operators
    /// </summary>
    public enum ConditionValueComparer
    {
        [Description("Equal To")]
        Equal = 0,

        [Description("Not Equal")]
        NotEqual = 1,

        [Description("Greater Than")]
        GreaterThan = 2,

        [Description("Less Than")]
        LessThan = 4,

        [Description("Greater Than Or Equal To")]
        GreaterThanOrEqual = 8,

        [Description("Less Than Or Equal To")]
        LessThanOrEqual = 8,
    }
}
