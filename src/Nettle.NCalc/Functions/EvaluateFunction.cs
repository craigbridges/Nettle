﻿namespace Nettle.NCalc.Functions
{
    using global::NCalc;
    using Nettle.Compiler;
    using Nettle.Functions;
    using System;
    using System.Linq;

    /// <summary>
    /// Represents function for evaluating a mathematical expression
    /// </summary>
    public class EvaluateFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public EvaluateFunction()
        {
            DefineRequiredParameter
            (
                "Expression",
                "The mathematical expression to evaluate",
                typeof(string)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Evaluates a single mathematical expression.";
            }
        }

        /// <summary>
        /// Generates an array of random integers
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The truncated text</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var expression = GetParameterValue<string>
            (
                "Expression",
                parameterValues
            );

            if (parameterValues.Length > 1)
            {
                var formatValues = parameterValues.Skip(1);

                expression = String.Format
                (
                    expression,
                    formatValues.ToArray()
                );
            }

            return new Expression(expression).Evaluate();
        }
    }
}
