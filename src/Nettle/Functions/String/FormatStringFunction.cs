namespace Nettle.Functions.String
{
    using System;

    public sealed class FormatStringFunction : FunctionBase
    {
        public FormatStringFunction() : base()
        {
            DefineRequiredParameter("Format", "The string to format.", typeof(string));
        }

        public override string Description =>
            "Replaces the format item in a specified string with the string " +
            "representation of a corresponding object in a specified array.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var format = GetParameterValue<string>("Format", parameterValues);
            var formatValues = parameterValues.Skip(1);

            return String.Format(format ?? String.Empty, formatValues.ToArray());
        }
    }
}
