namespace Nettle.Compiler.Parsing.Conditions
{
    /// <summary>
    /// Represents a boolean-valued expression that returns true or false
    /// </summary>
    internal class BooleanCondition
    {
        /// <summary>
        /// Constructs the condition with a left value only
        /// </summary>
        /// <param name="leftValue">The left value</param>
        public BooleanCondition
            (
                BooleanConditionValue leftValue
            )
        {
            Validate.IsNotNull(leftValue);

            this.LeftValue = leftValue;
        }

        /// <summary>
        /// Constructs the condition with left and right values
        /// </summary>
        /// <param name="leftValue">The left value</param>
        /// <param name="compareOperator">The compare operator</param>
        /// <param name="rightValue">The right value</param>
        public BooleanCondition
            (
                BooleanConditionValue leftValue,
                BooleanConditionOperator compareOperator,
                BooleanConditionValue rightValue
            )
        {
            Validate.IsNotNull(leftValue);
            Validate.IsNotNull(rightValue);

            this.LeftValue = leftValue;
            this.CompareOperator = compareOperator;
            this.RightValue = rightValue;
        }

        /// <summary>
        /// Constructs the condition with left and right values
        /// </summary>
        /// <param name="joinOperator">The join operator</param>
        /// <param name="leftValue">The left value</param>
        /// <param name="compareOperator">The compare operator</param>
        /// <param name="rightValue">The right value</param>
        public BooleanCondition
            (
                BooleanConditionOperator joinOperator,
                BooleanConditionValue leftValue,
                BooleanConditionOperator compareOperator,
                BooleanConditionValue rightValue
            )

            : this(leftValue, compareOperator, rightValue)
        {
            this.JoinOperator = joinOperator;
        }

        /// <summary>
        /// Gets the conditions join comparer
        /// </summary>
        /// <remarks>
        /// The join operator indicates how the condition is joined to 
        /// the previous condition.
        /// 
        /// E.g. With an expression "condition1 & condition2" 
        /// condition2 would have a join operator of And ("&")
        /// </remarks>
        public BooleanConditionOperator? JoinOperator { get; private set; }

        /// <summary>
        /// Gets the left side of the condition
        /// </summary>
        public BooleanConditionValue LeftValue { get; private set; }

        /// <summary>
        /// Gets the conditions value comparison comparer
        /// </summary>
        public BooleanConditionOperator? CompareOperator { get; private set; }

        /// <summary>
        /// Gets the right side of the condition
        /// </summary>
        public BooleanConditionValue RightValue { get; private set; }
    }
}
