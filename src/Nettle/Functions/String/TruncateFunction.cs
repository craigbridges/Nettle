namespace Nettle.Functions.String
{
    public sealed class TruncateFunction : FunctionBase
    {
        public TruncateFunction() : base()
        {
            DefineRequiredParameter("Text", "The text to truncate", typeof(string));
            DefineRequiredParameter("Length", "The texts maximum number of characters.", typeof(int));
        }

        public override string Description => "Truncates a string to the length specified.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var text = GetParameterValue<string>("Text", parameterValues);
            var length = GetParameterValue<int>("Length", parameterValues);

            return text?.Truncate(length);
        }
    }
}
