namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a variable incrementer renderer
    /// </summary>
    internal sealed class VariableIncrementerRenderer : VariableAdjusterRenderer<VariableIncrementer>
    {
        public VariableIncrementerRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        /// <summary>
        /// Gets the adjustment amount
        /// </summary>
        protected override int Adjustment => 1;
    }
}
