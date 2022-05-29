namespace Nettle.Compiler.Parsing.Conditions;

/// <summary>
/// Represents a boolean-valued expression that returns true or false
/// </summary>
/// <param name="LeftValue">The left side of the condition</param>
internal record class BooleanCondition(BooleanConditionValue LeftValue)
{
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
    public BooleanConditionOperator? JoinOperator { get; init; }

    /// <summary>
    /// Gets the conditions value comparison comparer
    /// </summary>
    public BooleanConditionOperator? CompareOperator { get; init; }

    /// <summary>
    /// Gets the right side of the condition
    /// </summary>
    public BooleanConditionValue? RightValue { get; init; }
}
