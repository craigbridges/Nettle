namespace Nettle.Functions.Math
{
    public sealed class SmallestNumberFunction : FunctionBase
    {
        public SmallestNumberFunction() 
            : base()
        { }

        public override string Description => "Gets the smallest number of a sequence.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var numbers = ConvertToNumbers(parameterValues);

            if (numbers.Length == 0)
            {
                throw new ArgumentException("The sequence does not contain any numbers.");
            }
            else
            {
                return numbers.OrderBy(number => number).First();
            }
        }
    }
}
