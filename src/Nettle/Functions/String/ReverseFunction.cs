namespace Nettle.Functions.String
{
    public sealed class ReverseFunction : FunctionBase
    {
        public ReverseFunction() : base()
        {
            DefineRequiredParameter("Text", "The text to reverse.", typeof(string));
        }

        public override string Description => "Reverses the order of a string.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var text = GetParameterValue<string>("Text", parameterValues);

            return new string(text?.Reverse().ToArray());
        }
    }
}
