namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class ParseDateFunction : FunctionBase
    {
        public ParseDateFunction() : base()
        {
            DefineRequiredParameter("Value", "The string value to parse.", typeof(string));
        }

        public override string Description => "Parses a string into a new date and time.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var value = GetParameterValue<string>("Value", parameterValues);

            return DateTime.Parse(value ?? String.Empty);
        }
    }
}
