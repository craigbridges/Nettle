namespace Nettle.NCalc.Functions
{
    using global::NCalc;
    using Nettle.Compiler;
    using Nettle.Functions;
    using System;
    using System.Linq;

    public class EvaluateFunction : FunctionBase
    {
        public EvaluateFunction()
        {
            DefineRequiredParameter("Expression", "The mathematical expression to evaluate", typeof(string));
        }

        public override string Description => "Evaluates a single mathematical expression.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var expression = GetParameterValue<string>("Expression", parameterValues);

            if (parameterValues.Length > 1)
            {
                var formatValues = parameterValues.Skip(1);

                expression = String.Format(expression ?? String.Empty, formatValues.ToArray());
            }

            return new Expression(expression).Evaluate();
        }
    }
}
