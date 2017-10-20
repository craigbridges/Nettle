namespace Nettle.Compiler.Parsing.Conditions
{
    /// <summary>
    /// Represents a structure that manages a boolean expression
    /// </summary>
    internal class BooleanExpression
    {
        /// <summary>
        /// Constructs the expression with the details
        /// </summary>
        /// <param name="expression">he unparsed expression</param>
        /// <param name="conditions">The boolean conditions</param>
        public BooleanExpression
            (
                string expression,
                BooleanCondition[] conditions
            )
        {
            Validate.IsNotEmpty(expression);
            Validate.IsNotNull(conditions);

            this.Expression = expression;
            this.Conditions = conditions;
        }

        /// <summary>
        /// Gets the boolean expressions unparsed expression
        /// </summary>
        public string Expression { get; private set; }

        /// <summary>
        /// Gets an array of boolean conditions
        /// </summary>
        public BooleanCondition[] Conditions { get; private set; }
    }
}
