namespace Nettle.Compiler.Rendering
{
    using Nettle.Compiler.Parsing.Conditions;
    using Nettle.Functions;
    using System;

    /// <summary>
    /// Represents a boolean expression evaluator
    /// </summary>
    internal sealed class BooleanExpressionEvaluator : NettleRendererBase
    {
        /// <summary>
        /// Constructs the expression evaluator with required dependencies
        /// </summary>
        /// <param name="functionRepository">The function repository</param>
        public BooleanExpressionEvaluator
            (
                IFunctionRepository functionRepository
            )

            : base(functionRepository)
        { }

        /// <summary>
        /// Evaluates a boolean expression against a template context
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="expression">The expression</param>
        /// <returns>The evaluation result</returns>
        public bool Evaluate
            (
                ref TemplateContext context,
                BooleanExpression expression
            )
        {
            Validate.IsNotNull(expression);

            var expressionResult = false;
            var previousConditionResult = false;

            foreach (var condition in expression.Conditions)
            {
                var conditionResult = Evaluate
                (
                    ref context,
                    condition
                );

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
        /// Evaluates a boolean condition against a template context
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="condition">The condition</param>
        /// <returns>The evaluation result</returns>
        private bool Evaluate
            (
                ref TemplateContext context,
                BooleanCondition condition
            )
        {
            Validate.IsNotNull(condition);

            var result = false;

            if (condition.RightValue == null)
            {
                result = Evaluate
                (
                    ref context,
                    condition.LeftValue
                );
            }
            else
            {
                var leftValue = ResolveValue
                (
                    ref context,
                    condition.LeftValue.Value,
                    condition.LeftValue.ValueType
                );

                var rightValue = ResolveValue
                (
                    ref context,
                    condition.RightValue.Value,
                    condition.RightValue.ValueType
                );

                switch (condition.CompareOperator)
                {
                    case BooleanConditionOperator.And:
                        {
                            var leftResult = ToBool(ref context, leftValue);
                            var rightResult = ToBool(ref context, rightValue);

                            result = (leftResult && rightResult);
                            break;
                        }

                    case BooleanConditionOperator.Or:
                        {
                            var leftResult = ToBool(ref context, leftValue);
                            var rightResult = ToBool(ref context, rightValue);

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
        /// Evaluates a boolean condition value against a template context
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="conditionValue">The condition value</param>
        /// <returns>The evaluation result</returns>
        private bool Evaluate
            (
                ref TemplateContext context,
                BooleanConditionValue conditionValue
            )
        {
            Validate.IsNotNull(conditionValue);
            
            var resolvedValue = ResolveValue
            (
                ref context,
                conditionValue.Value,
                conditionValue.ValueType
            );

            return ToBool
            (
                ref context,
                resolvedValue
            );
        }

        /// <summary>
        /// Compares two values and determines if they are considered equal
        /// </summary>
        /// <param name="value1">The first value</param>
        /// <param name="value2">The second value</param>
        /// <returns>True, if both values are equal; otherwise false</returns>
        private bool Compare
            (
                object value1,
                object value2
            )
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
        private double ToNumber
            (
                object value
            )
        {
            var result = default(double);

            if (value != null)
            {
                if (value is double || value.GetType().IsNumeric())
                {
                    result = Convert.ToDouble(value);
                }
                else if (value.GetType() == typeof(string))
                {
                    Double.TryParse
                    (
                        (string)value,
                        out double number
                    );

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
        /// Converts an object into a boolean representation
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="value">The value to convert</param>
        /// <returns>The boolean representation</returns>
        private bool ToBool
            (
                ref TemplateContext context,
                object value
            )
        {
            var result = default(bool);

            if (value != null)
            {
                if (value is bool)
                {
                    result = (bool)value;
                }
                else if (value is BooleanExpression)
                {
                    return Evaluate
                    (
                        ref context,
                        (BooleanExpression)value
                    );
                }
                else if (value.GetType().IsNumeric())
                {
                    result = (double)value > 0;
                }
                else
                {
                    result = value.ToString().Length > 0;
                }
            }

            return result;
        }
    }
}
