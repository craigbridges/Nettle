namespace Nettle.Parsing
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

        [Description("Model Binding")]
        ModelBinding = 2,

        [Description("Variable")]
        Variable = 4,

        [Description("Function Call")]
        Function = 8
    }
}
