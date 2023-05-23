namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing.Conditions;
using System.Threading.Tasks;

internal sealed class BooleanExpressionEvaluator : NettleRendererBase
{
    public BooleanExpressionEvaluator(IFunctionRepository functionRepository)
        : base(functionRepository)
    { }

    /// <summary>
    /// Asynchronously evaluates a boolean expression against a template context
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="expression">The expression</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The evaluation result</returns>
    public async Task<bool> Evaluate(TemplateContext context, BooleanExpression expression, CancellationToken cancellationToken)
    {
        var expressionResult = false;
        var previousConditionResult = true;

        foreach (var condition in expression.Conditions)
        {
            var conditionResult = await Evaluate(context, condition, cancellationToken);

            if (condition.JoinOperator.HasValue)
            {
                switch (condition.JoinOperator.Value)
                {
                    case BooleanConditionOperator.And:
                    {
                        if (previousConditionResult && conditionResult)
                        {
                            expressionResult = true;
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    }
                    case BooleanConditionOperator.Or:
                    {
                        if (previousConditionResult || conditionResult)
                        {
                            expressionResult = true;
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    }
                }
            }
            else
            {
                expressionResult = conditionResult;
            }

            previousConditionResult = conditionResult;
        }

        return expressionResult;
    }

    /// <summary>
    /// Asynchronously evaluates a boolean condition against a template context
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="condition">The condition</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The evaluation result</returns>
    private async Task<bool> Evaluate(TemplateContext context, BooleanCondition condition, CancellationToken cancellationToken)
    {
        var result = false;

        if (condition.RightValue == null)
        {
            result = await Evaluate(context, condition.LeftValue, cancellationToken);
        }
        else
        {
            var leftCondition = condition.LeftValue;
            var leftValue = await ResolveValue(context, leftCondition.Value, leftCondition.ValueType, cancellationToken);

            var rightCondition = condition.RightValue;
            var rightValue = await ResolveValue(context, rightCondition.Value, rightCondition.ValueType, cancellationToken);

            var @operator = condition.CompareOperator ?? condition.JoinOperator;

            switch (@operator)
            {
                case BooleanConditionOperator.And:
                {
                    var leftResult = await ToBool(context, leftValue, cancellationToken);
                    var rightResult = await ToBool(context, rightValue, cancellationToken);

                    result = (leftResult && rightResult);
                    break;
                }
                case BooleanConditionOperator.Or:
                {
                    var leftResult = await ToBool(context, leftValue, cancellationToken);
                    var rightResult = await ToBool(context, rightValue, cancellationToken);

                    result = (leftResult || rightResult);
                    break;
                }
                case BooleanConditionOperator.Equal:
                {
                    result = Compare(leftValue, rightValue);
                    break;
                }
                case BooleanConditionOperator.NotEqual:
                {
                    result = (false == Compare(leftValue, rightValue));
                    break;
                }
                case BooleanConditionOperator.GreaterThan:
                {
                    var leftNumber = ToNumber(leftValue);
                    var rightNumber = ToNumber(rightValue);

                    result = (leftNumber > rightNumber);
                    break;
                }
                case BooleanConditionOperator.GreaterThanOrEqual:
                {
                    var leftNumber = ToNumber(leftValue);
                    var rightNumber = ToNumber(rightValue);

                    result = (leftNumber >= rightNumber);
                    break;
                }
                case BooleanConditionOperator.LessThan:
                {
                    var leftNumber = ToNumber(leftValue);
                    var rightNumber = ToNumber(rightValue);

                    result = (leftNumber < rightNumber);
                    break;
                }
                case BooleanConditionOperator.LessThanOrEqual:
                {
                    var leftNumber = ToNumber(leftValue);
                    var rightNumber = ToNumber(rightValue);

                    result = (leftNumber <= rightNumber);
                    break;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Asynchronously evaluates a boolean condition value against a template context
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="conditionValue">The condition value</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The evaluation result</returns>
    private async Task<bool> Evaluate(TemplateContext context, BooleanConditionValue conditionValue, CancellationToken cancellationToken)
    {
        var resolvedValue = await ResolveValue(context, conditionValue.Value, conditionValue.ValueType, cancellationToken);

        return await ToBool(context, resolvedValue, cancellationToken);
    }

    /// <summary>
    /// Compares two values and determines if they are considered equal
    /// </summary>
    /// <param name="value1">The first value</param>
    /// <param name="value2">The second value</param>
    /// <returns>True, if both values are equal; otherwise false</returns>
    private static bool Compare(object? value1, object? value2)
    {
        if (value1 == null && value2 == null)
        {
            return true;
        }
        else if (value1 == null || value2 == null)
        {
            return false;
        }
        else if (value1.GetType().IsNumeric() && value2.GetType().IsNumeric())
        {
            var number1 = ToNumber(value1);
            var number2 = ToNumber(value2);

            return number1 == number2;
        }
        else
        {
            return value1.Equals(value2);
        }
    }

    /// <summary>
    /// Converts an object into a double representation
    /// </summary>
    /// <param name="value">The value to convert</param>
    /// <returns>The double representation</returns>
    private static double ToNumber(object? value)
    {
        var result = default(double);

        if (value != null)
        {
            var type = value.GetType();

            if (value is double || type.IsNumeric())
            {
                result = Convert.ToDouble(value);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return ((DateTime)value).Ticks;
            }
            else if (type == typeof(string))
            {
                _ = Double.TryParse((string)value, out double number);

                return number;
            }
            else
            {
                return 0;
            }
        }

        return result;
    }

    /// <summary>
    /// Asynchronously converts an object into a boolean representation
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="value">The value to convert</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The boolean representation</returns>
    private async Task<bool> ToBool(TemplateContext context, object? value, CancellationToken cancellationToken)
    {
        var result = default(bool);

        if (value != null)
        {
            if (value is bool boolean)
            {
                result = boolean;
            }
            else if (value is BooleanExpression expression)
            {
                return await Evaluate(context, expression, cancellationToken);
            }
            else if (value.GetType().IsNumeric())
            {
                result = Convert.ToInt32(value) > 0;
            }
            else
            {
                result = value.ToString()?.Length > 0;
            }
        }

        return result;
    }
}
