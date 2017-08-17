namespace Nettle.Parsing
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a collection of function call parameter types
    /// </summary>
    internal enum FunctionCallParameterType
    {
        [Description("String")]
        String = 0,

        [Description("Number")]
        Number = 1,

        [Description("Model Binding")]
        ModelBinding = 2,

        [Description("Variable")]
        Variable = 4
    }
}
