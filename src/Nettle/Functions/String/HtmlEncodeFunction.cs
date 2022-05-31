namespace Nettle.Functions.String
{
    using System;
    using System.Web;

    public sealed class HtmlEncodeFunction : FunctionBase
    {
        public HtmlEncodeFunction() : base()
        {
            DefineRequiredParameter("Text", "The text to encode.", typeof(string));
        }

        public override string Description => "HTML encodes text.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var text = GetParameterValue<string>("Text", parameterValues);

            return HttpUtility.HtmlEncode(text ?? String.Empty);
        }
    }
}
