namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Blocks;
    using Nettle.Functions;

    /// <summary>
    /// Represents a variable incrementer renderer
    /// </summary>
    internal sealed class VariableIncrementerRenderer 
        : VariableAdjusterRenderer<VariableIncrementer>
    {
        /// <summary>
        /// Constructs the renderer with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public VariableIncrementerRenderer
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
                return 1;
            }
        }
    }
}
