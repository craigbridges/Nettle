namespace Nettle.Compiler.Parsing.Conditions
{
    internal static class BooleanConditionOperatorExtensions
    {
        /// <summary>
        /// Determines if the operator is a joining operator (such as AND/OR)
        /// </summary>
        /// <param name="operator">The operator to evaluate</param>
        /// <returns>True, if the operator is a join</returns>
        public static bool IsJoin(this BooleanConditionOperator @operator)
        {
            return @operator == BooleanConditionOperator.And || @operator == BooleanConditionOperator.Or;
        }
    }
}
