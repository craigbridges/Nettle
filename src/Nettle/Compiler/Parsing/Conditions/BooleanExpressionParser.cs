namespace Nettle.Compiler.Parsing.Conditions;

/// <summary>
/// Represents a boolean expression parser
/// </summary>
internal sealed class BooleanExpressionParser : NettleParser
{
    private readonly Tokenizer _tokenizer;
    private readonly Dictionary<string, BooleanConditionOperator> _operatorLookup;

    public BooleanExpressionParser()
    {
        _tokenizer = new Tokenizer();

        _operatorLookup = new Dictionary<string, BooleanConditionOperator>()
        {
            { "==", BooleanConditionOperator.Equal },
            { "!=", BooleanConditionOperator.NotEqual },
            { ">", BooleanConditionOperator.GreaterThan },
            { "<", BooleanConditionOperator.LessThan },
            { ">=", BooleanConditionOperator.GreaterThanOrEqual },
            { "<=", BooleanConditionOperator.LessThanOrEqual },
            { "&", BooleanConditionOperator.And },
            { "|", BooleanConditionOperator.Or }
        };
    }

    /// <summary>
    /// Parses the expression specified into a boolean expression
    /// </summary>
    /// <param name="expression">The expression to parse</param>
    /// <returns>The parsed boolean expression</returns>
    /// <remarks>
    /// The expression is made of alternating values and operators.
    /// 
    /// The following example demonstrates a simple expression:
    /// 
    /// "active == true & count > 5 | index != 0"
    /// 
    /// If we assume the following values:
    /// 
    /// action = true
    /// count = 4
    /// index = 1
    /// 
    /// Then the expression would evaluate to true.
    /// </remarks>
    public BooleanExpression Parse(string expression)
    {
        Validate.IsNotEmpty(expression);

        // Strip any wrapping brackets that are found
        if (expression.StartsWith("(") && expression.EndsWith(""))
        {
            expression = expression.Crop(1, expression.Length - 2);
        }

        var tokens = _tokenizer.Tokenize(expression);
        var conditions = new List<BooleanCondition>();
        var operatorExpected = false;

        // Handle situations where the expression contains a single value
        if (tokens.Length == 1)
        {
            var token = tokens[0];
            var valueType = ResolveType(token);
            var value = valueType.ParseValue(token);

            conditions.Add(new BooleanCondition(new BooleanConditionValue(token, valueType, value)));
        }
        else
        {
            var currentJoinOperator = default(BooleanConditionOperator?);
            var currentLeftValue = default(BooleanConditionValue);
            var currentCompareOperator = default(BooleanConditionOperator?);

            foreach (var token in tokens)
            {
                if (operatorExpected)
                {
                    var isOperator = IsOperator(token);

                    if (false == isOperator)
                    {
                        throw new NettleParseException
                        (
                            $"The boolean expression '{expression}' is invalid. " +
                            $"A valid operator was expected, but not found."
                        );
                    }

                    var @operator = ResolveOperator(token);
                    var isJoin = @operator.IsJoin();

                    if ((isJoin && currentJoinOperator != null) || (currentCompareOperator != null))
                    {
                        throw new NettleParseException
                        (
                            $"The boolean expression '{expression}' is invalid. " +
                            $"The operator {@operator} is not allowed here."
                        );
                    }
                    else if (isJoin)
                    {
                        currentJoinOperator = @operator;
                    }
                    else
                    {
                        currentCompareOperator = @operator;
                    }
                }
                else
                {
                    var valueType = ResolveType(token);
                    var value = valueType.ParseValue(token);

                    if (currentLeftValue == null)
                    {
                        currentLeftValue = new BooleanConditionValue(token, valueType, value);
                    }
                    else
                    {
                        BooleanConditionValue? currentRightValue = new(token, valueType, value);

                        if (currentJoinOperator.HasValue)
                        {
                            conditions.Add
                            (
                                new BooleanCondition(currentLeftValue)
                                {
                                    JoinOperator = currentJoinOperator,
                                    CompareOperator = currentCompareOperator
                                }
                            );
                        }
                        else
                        {
                            conditions.Add
                            (
                                new BooleanCondition(currentLeftValue)
                                {
                                    CompareOperator = currentCompareOperator,
                                    RightValue = currentRightValue
                                }
                            );
                        }

                        currentLeftValue = null;
                        currentCompareOperator = null;
                        currentRightValue = null;
                    }
                }

                operatorExpected = !operatorExpected;
            }

            // Add the condition if no right side was found
            // This will happen when an odd number of conditions are found
            if (currentLeftValue != null)
            {
                if (currentJoinOperator.HasValue)
                {
                    conditions.Add
                    (
                        new BooleanCondition(currentLeftValue)
                        {
                            JoinOperator = currentJoinOperator
                        }
                    );
                }
                else
                {
                    conditions.Add(new BooleanCondition(currentLeftValue));
                }
            }
        }

        return new BooleanExpression(expression, conditions.ToArray());
    }

    /// <summary>
    /// Determines if a token is a condition operator
    /// </summary>
    /// <param name="token">The token to check</param>
    /// <returns>True, if the token is an operator; otherwise false</returns>
    private bool IsOperator(string token) => _operatorLookup.ContainsKey(token);

    /// <summary>
    /// Resolves the boolean condition operator from a token
    /// </summary>
    /// <param name="token">The token value</param>
    /// <returns>The resolved operator</returns>
    private BooleanConditionOperator ResolveOperator(string token)
    {
        var found = _operatorLookup.ContainsKey(token);

        if (false == found)
        {
            throw new NettleParseException($"'{token}' is not a valid boolean operator.");
        }

        return _operatorLookup[token];
    }
}
