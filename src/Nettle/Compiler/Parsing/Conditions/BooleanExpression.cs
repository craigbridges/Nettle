namespace Nettle.Compiler.Parsing.Conditions
{
    /// <summary>
    /// Represents a structure that manages a boolean expression
    /// </summary>
    /// <param name="Expression">The boolean expressions unparsed expression</param>
    /// <param name="Conditions">An array of boolean conditions</param>
    internal record class BooleanExpression(string Expression, BooleanCondition[] Conditions)
    {
        /// <summary>
        /// Provides a custom string representation of the expression
        /// </summary>
        /// <returns>The raw expression</returns>
        public override string ToString() => Expression;
    }
}
