namespace Nettle.Functions.Math
{
    public sealed class SumFunction : FunctionBase
    {
        public SumFunction() 
            : base()
        { }

        public override string Description => "Computes the sum of a sequence of numeric values.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var numbers = ConvertToNumbers(parameterValues);
            var total = numbers.Sum();

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
