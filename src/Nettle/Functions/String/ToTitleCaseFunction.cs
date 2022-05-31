namespace Nettle.Functions.String
{
    public sealed class ToTitleCaseFunction : FunctionBase
    {
        public ToTitleCaseFunction() : base()
        {
            DefineRequiredParameter("Text", "The text to convert.", typeof(string));
        }

        public override string Description => "Converts a string to title case.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var text = GetParameterValue<string>("Text", parameterValues);

            return text?.ToTitleCase();
        }
    }
}
