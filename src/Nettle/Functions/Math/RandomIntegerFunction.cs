namespace Nettle.Functions.Math
{
    public sealed class RandomIntegerFunction : FunctionBase
    {
        private static readonly Random _random = new();

        public RandomIntegerFunction() : base()
        {
            DefineRequiredParameter("MinValue", "The minimum value.", typeof(int));
            DefineRequiredParameter("MaxValue", "The maximum value.", typeof(int));
        }

        public override string Description => "Generates a random integer between a range.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var minValue = GetParameterValue<int>("MinValue", parameterValues);
            var maxValue = GetParameterValue<int>("MaxValue", parameterValues);
            
            return _random.Next(minValue, maxValue);
        }
    }
}
