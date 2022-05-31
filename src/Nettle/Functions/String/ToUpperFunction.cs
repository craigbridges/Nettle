namespace Nettle.Functions.String
{
    public sealed class ToUpperFunction : FunctionBase
    {
        public ToUpperFunction() : base()
        {
            DefineRequiredParameter("Text", "The text to convert.", typeof(string));
        }

        public override string Description => "Converts a string to upper case.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var text = GetParameterValue<string>("Text", parameterValues);

            return text?.ToUpper();
        }
    }
}
