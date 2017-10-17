namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;

    /// <summary>
    /// Represents a variable decrementer renderer
    /// </summary>
    internal sealed class VariableDecrementerRenderer 
        : VariableAdjusterRenderer<VariableDecrementer>
    {
        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public VariableDecrementerRenderer
            (
                IFunctionRepository functionRepository
            )

            : base(functionRepository)
        { }

        /// <summary>
        /// Gets the adjustment amount
        /// </summary>
        protected override int Adjustment
        {
            get
            {
                return -1;
            }
        }
    }
}
