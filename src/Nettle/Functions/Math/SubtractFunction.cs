﻿namespace Nettle.Functions.Math
{
    public sealed class SubtractFunction : FunctionBase
    {
        public SubtractFunction() : base()
        {
            DefineRequiredParameter("NumberOne", "The first number.", typeof(double));
            DefineRequiredParameter("NumberTwo", "The second number.", typeof(double));
        }

        public override string Description => "Subtracts two numbers from each other.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var number1 = GetParameterValue<double>("NumberOne", parameterValues);
            var number2 = GetParameterValue<double>("NumberTwo", parameterValues);

            var total = (number1 - number2);

            if (total.IsWholeNumber())
            {
                return Convert.ToInt64(total);
            }
            else
            {
                return total;
            }
        }
    }
}
