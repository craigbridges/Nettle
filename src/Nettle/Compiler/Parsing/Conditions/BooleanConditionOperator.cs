namespace Nettle.Compiler.Parsing.Conditions
{
    using System.ComponentModel;

    /// <summary>
    /// Defines condition operators
    /// </summary>
    public enum BooleanConditionOperator
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
        LessThanOrEqual = 16,

        [Description("And")]
        And = 32,

        [Description("Or")]
        Or = 64
    }
}
