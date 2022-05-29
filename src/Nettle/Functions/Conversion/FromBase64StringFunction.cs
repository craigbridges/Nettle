namespace Nettle.Functions.Conversion
{
    /// <summary>
    /// Represent a convert base-64 string to byte array function implementation
    /// </summary>
    public sealed class FromBase64StringFunction : FunctionBase
    {
        public FromBase64StringFunction() : base()
        {
            DefineRequiredParameter("Data", "The base-64 encoded string.", typeof(string));
        }

        public override string Description => "Converts a base-64 encoded string to a byte array.";

        protected override object GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var data = GetParameterValue<string>("Data", parameterValues);

            return Convert.FromBase64String(data ?? string.Empty);
        }
    }
}
