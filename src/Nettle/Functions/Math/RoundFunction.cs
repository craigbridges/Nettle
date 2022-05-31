namespace Nettle.Functions.Math
{
    using System;

    public sealed class RoundFunction : FunctionBase
    {
        public RoundFunction() : base()
        {
            DefineRequiredParameter("Number", "The number to round", typeof(double));
            DefineRequiredParameter("Decimals", "The number of decimal places.", typeof(int));
        }

        public override string Description => "Rounds a number to a set number of decimal places.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var number = GetParameterValue<double>("Number", parameterValues);
            var decimals = GetParameterValue<int>("Decimals", parameterValues);

            return Math.Round(number, decimals);
        }
    }
}
