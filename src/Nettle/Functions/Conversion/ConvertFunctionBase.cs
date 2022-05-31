namespace Nettle.Functions.Conversion
{
    /// <summary>
    /// Represent a convert object to type function base class
    /// </summary>
    /// <typeparam name="TTo">The object type to convert to</typeparam>
    public abstract class ConvertFunctionBase<TTo> : FunctionBase
    {
        public ConvertFunctionBase() : base()
        {
            DefineRequiredParameter("Value", "The object value to convert.", typeof(object));
        }

        public override string Description => $"Converts an object to a {typeof(TTo).Name}.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var value = GetParameterValue<object?>("Value", parameterValues);
            var converter = new GenericObjectToTypeConverter<TTo>();

            return converter.Convert(value);
        }
    }
}
