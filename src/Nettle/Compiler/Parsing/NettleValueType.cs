namespace Nettle.Compiler.Parsing
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a collection of Nettle value types
    /// </summary>
    internal enum NettleValueType
    {
        [Description("String")]
        String = 0,

        [Description("Number")]
        Number = 1,

        [Description("Boolean")]
        Boolean = 2,

        [Description("Model Binding")]
        ModelBinding = 4,

        [Description("Variable")]
        Variable = 8,

        [Description("Function Call")]
        Function = 16,

        [Description("Boolean Expression")]
        BooleanExpression = 32
    }
}
