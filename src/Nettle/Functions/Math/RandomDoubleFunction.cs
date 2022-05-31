namespace Nettle.Functions.Math
{
    public sealed class RandomDoubleFunction : FunctionBase
    {
        private static readonly Random _random = new();

        public RandomDoubleFunction() : base()
        {
            DefineRequiredParameter("MinValue", "The minimum value.", typeof(double));
            DefineRequiredParameter("MaxValue", "The maximum value.", typeof(double));
        }

        public override string Description => "Generates a random double between a range.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var minValue = GetParameterValue<double>("MinValue", parameterValues);
            var maxValue = GetParameterValue<double>("MaxValue", parameterValues);

            var nextDouble = _random.NextDouble();
            var number = (nextDouble * (maxValue - minValue) + minValue);

            return number;
        }
    }
}
