namespace Nettle.Compiler.Parsing.Conditions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a boolean expression parser
    /// </summary>
    internal sealed class BooleanExpressionParser : NettleParser
    {
        private Dictionary<string, BooleanConditionOperator> _operatorLookup;

        /// <summary>
        /// Constructs the parser by initialising the operator lookup
        /// </summary>
        public BooleanExpressionParser()
        {
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
        public BooleanExpression Parse
            (
                string expression
            )
        {
            Validate.IsNotEmpty(expression);
            
            var tokens = Tokenize(expression);
            var conditions = new List<BooleanCondition>();
            var operatorExpected = false;

            // Handle situations where the expression contains a single value
            if (tokens.Length == 1)
            {
                var token = tokens[0];
                var valueType = this.ResolveType(token);
                var value = valueType.ParseValue(token);

                conditions.Add
                (
                    new BooleanCondition
                    (
                        new BooleanConditionValue
                        (
                            token,
                            valueType,
                            value
                        )
                    )
                );
            }
            else
            {
                var currentJoinOperator = default(BooleanConditionOperator?);
                var currentLeftValue = default(BooleanConditionValue);
                var currentCompareOperator = default(BooleanConditionOperator?);
                var currentRightValue = default(BooleanConditionValue);

                foreach (var token in tokens)
                {
                    if (operatorExpected)
                    {
                        var isOperator = IsOperator(token);

                        if (false == isOperator)
                        {
                            var message = "The expression '{0}' is invalid. " +
                                          "An operator was expected, but not found.";

                            throw new NettleParseException
                            (
                                message.With
                                (
                                    expression
                                )
                            );
                        }

                        var @operator = ResolveOperator(token);

                        if (conditions.Any() && currentJoinOperator == null)
                        {
                            switch (@operator)
                            {
                                case BooleanConditionOperator.And:
                                case BooleanConditionOperator.Or:
                                    break;

                                default:
                                    var message = "The expression '{0}' is invalid. " +
                                                  "The operator {1} is not allowed here.";

                                    throw new NettleParseException
                                    (
                                        message.With
                                        (
                                            expression,
                                            @operator
                                        )
                                    );
                            }

                            currentJoinOperator = @operator;
                        }
                        else
                        {
                            currentCompareOperator = @operator;
                        }
                    }
                    else
                    {
                        var valueType = this.ResolveType(token);
                        var value = valueType.ParseValue(token);

                        if (currentLeftValue == null)
                        {
                            currentLeftValue = new BooleanConditionValue
                            (
                                token,
                                valueType,
                                value
                            );
                        }
                        else
                        {
                            currentRightValue = new BooleanConditionValue
                            (
                                token,
                                valueType,
                                value
                            );

                            if (currentJoinOperator.HasValue)
                            {
                                conditions.Add
                                (
                                    new BooleanCondition
                                    (
                                        currentJoinOperator.Value,
                                        currentLeftValue,
                                        currentCompareOperator.Value,
                                        currentRightValue
                                    )
                                );
                            }
                            else
                            {
                                conditions.Add
                                (
                                    new BooleanCondition
                                    (
                                        currentLeftValue,
                                        currentCompareOperator.Value,
                                        currentRightValue
                                    )
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
                            new BooleanCondition
                            (
                                currentJoinOperator.Value,
                                currentLeftValue
                            )
                        );
                    }
                    else
                    {
                        conditions.Add
                        (
                            new BooleanCondition
                            (
                                currentLeftValue
                            )
                        );
                    }
                }
            }

            return new BooleanExpression
            (
                expression,
                conditions.ToArray()
            );
        }

        /// <summary>
        /// Splits the expression into individual tokens
        /// </summary>
        /// <param name="expression">The expression</param>
        /// <returns>An array of tokens</returns>
        private string[] Tokenize
            (
                string expression
            )
        {
            // Remove extra white space
            expression = expression.Trim().Replace
            (
                "  ",
                " "
            );

            var tokens = new List<string>();
            var tokenBuilder = new StringBuilder();
            var separatorQueue = new Stack<char>();

            // Define the token separates
            var separators = new Dictionary<char, char>()
            {
                { '"', '"' },
                { '(', ')' }
            };

            foreach (var c in expression)
            {
                if (tokenBuilder.Length == 0)
                {
                    // Start a new token
                    tokenBuilder.Append(c);

                    if (separators.ContainsKey(c))
                    {
                        separatorQueue.Push
                        (
                            separators[c]
                        );
                    }
                    else
                    {
                        separatorQueue.Push(' ');
                    }
                }
                else
                {
                    var tokenComplete = false;
                    
                    // Check if this character is a separator
                    if (separatorQueue.Peek() == c)
                    {
                        separatorQueue.Pop();

                        if (separatorQueue.Count == 0)
                        {
                            tokenComplete = true;
                        }
                    }
                    else if (separators.ContainsKey(c))
                    {
                        separatorQueue.Push
                        (
                            separators[c]
                        );
                    }

                    tokenBuilder.Append(c);

                    // Check if we should flush the current token
                    if (tokenComplete)
                    {
                        tokens.Add
                        (
                            tokenBuilder.ToString().Trim()
                        );

                        tokenBuilder.Clear();
                    }
                }
            }

            if (tokenBuilder.Length > 0)
            {
                tokens.Add
                (
                    tokenBuilder.ToString().Trim()
                );
            }

            return tokens.ToArray();
        }

        /// <summary>
        /// Determines if a token is a condition operator
        /// </summary>
        /// <param name="token">The token to check</param>
        /// <returns>True, if the token is an operator; otherwise false</returns>
        private bool IsOperator
            (
                string token
            )
        {
            return _operatorLookup.ContainsKey
            (
                token
            );
        }

        /// <summary>
        /// Resolves the boolean condition operator from a token
        /// </summary>
        /// <param name="token">The token value</param>
        /// <returns>The resolved operator</returns>
        private BooleanConditionOperator ResolveOperator
            (
                string token
            )
        {
            var found = _operatorLookup.ContainsKey
            (
                token
            );

            if (false == found)
            {
                throw new NettleParseException
                (
                    "The token '{0}' is not a valid operator."
                );
            }

            return _operatorLookup[token];
        }
    }
}
