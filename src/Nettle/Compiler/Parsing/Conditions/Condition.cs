namespace Nettle.Compiler.Parsing.Conditions
{
    /// <summary>
    /// Represents a boolean-valued expression that returns true or false
    /// </summary>
    internal class Condition
    {
        /// <summary>
        /// Constructs the condition with a left value only
        /// </summary>
        /// <param name="leftValue">The left value</param>
        public Condition
            (
                ConditionValue leftValue
            )
        {
            Validate.IsNotNull(leftValue);

            this.LeftValue = leftValue;
        }

        /// <summary>
        /// Constructs the condition with left and right values
        /// </summary>
        /// <param name="leftValue">The left value</param>
        /// <param name="comparer">The value comparer</param>
        /// <param name="rightValue">The right value</param>
        public Condition
            (
                ConditionValue leftValue,
                ConditionValueComparer comparer,
                ConditionValue rightValue
            )
        {
            Validate.IsNotNull(leftValue);
            Validate.IsNotNull(rightValue);

            this.LeftValue = leftValue;
            this.Comparer = comparer;
            this.RightValue = rightValue;
        }

        /// <summary>
        /// Gets the left side of the condition
        /// </summary>
        public ConditionValue LeftValue { get; private set; }

        /// <summary>
        /// Gets the conditions value comparer
        /// </summary>
        public ConditionValueComparer? Comparer { get; private set; }

        /// <summary>
        /// Gets the right side of the condition
        /// </summary>
        public ConditionValue RightValue { get; private set; }
    }
}
