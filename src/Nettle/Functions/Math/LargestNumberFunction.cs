namespace Nettle.Functions.Math
{
    public sealed class LargestNumberFunction : FunctionBase
    {
        public LargestNumberFunction() 
            : base()
        { }

        public override string Description => "Gets the largest number of a sequence.";

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
                return numbers.OrderByDescending(number => number).First();
            }
        }
    }
}
