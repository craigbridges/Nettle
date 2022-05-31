namespace Nettle.Functions.String
{
    public sealed class ToLowerFunction : FunctionBase
    {
        public ToLowerFunction() : base()
        {
            DefineRequiredParameter("Text", "The text to convert.", typeof(string));
        }

        public override string Description => "Converts a string to lower case.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var text = GetParameterValue<string>("Text", parameterValues);

            return text?.ToLower();
        }
    }
}
