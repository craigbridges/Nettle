namespace Nettle.Functions.String
{
    using System;

    public sealed class ReplaceFunction : FunctionBase
    {
        public ReplaceFunction() : base()
        {
            DefineRequiredParameter("Text", "The original text", typeof(string));
            DefineRequiredParameter("OldValue", "The old value", typeof(string));
            DefineRequiredParameter("NewValue", "The new value", typeof(string));
        }

        public override string Description => "Replaces text in a string with other text.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var text = GetParameterValue<string>("Text", parameterValues);
            var oldValue = GetParameterValue<string>("OldValue", parameterValues);
            var newValue = GetParameterValue<string>("NewValue", parameterValues);

            return text?.Replace(oldValue ?? String.Empty, newValue);
        }
    }
}
