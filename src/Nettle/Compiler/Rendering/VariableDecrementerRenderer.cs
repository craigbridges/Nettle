namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a variable decrementer renderer
    /// </summary>
    internal sealed class VariableDecrementerRenderer : VariableAdjusterRenderer<VariableDecrementer>
    {
        public VariableDecrementerRenderer(IFunctionRepository functionRepository)
            : base(functionRepository)
        { }

        /// <summary>
        /// Gets the adjustment amount
        /// </summary>
        protected override int Adjustment => -1;
    }
}
