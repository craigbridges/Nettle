namespace Nettle.Compiler.Parsing
{
    using Nettle.Compiler.Parsing.Blocks;

    /// <summary>
    /// Represents a variable decrementer block parser
    /// </summary>
    internal sealed class VariableDecrementerParser : VariableAdjusterParser
    {
        /// <summary>
        /// Gets the decrementer operator signature
        /// </summary>
        protected override string AdjusterSignature
        {
            get
            {
                return @"--";
            }
        }

        /// <summary>
        /// Overrides the base parse logic to return an decrementer code block
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <param name="positionOffSet">The position offset index</param>
        /// <param name="signature">The block signature</param>
        /// <returns>The parsed code block</returns>
        public override CodeBlock Parse
            (
                ref string templateContent,
                ref int positionOffSet,
                string signature
            )
        {
            var adjuster = (VariableAdjuster)base.Parse
            (
                ref templateContent,
                ref positionOffSet,
                signature
            );

            return new VariableDecrementer()
            {
                Signature = adjuster.Signature,
                StartPosition = adjuster.StartPosition,
                EndPosition = adjuster.EndPosition,
                VariableName = adjuster.VariableName
            };
        }
    }
}
