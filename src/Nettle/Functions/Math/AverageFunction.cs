namespace Nettle.Functions.Math
{
    using System.Linq;

    public sealed class AverageFunction : FunctionBase
    {
        public AverageFunction() 
            : base()
        { }

        public override string Description => "Computes the average from a sequence of numeric values.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var numbers = ConvertToNumbers(parameterValues);
            
            return numbers.Average();
        }
    }
}
