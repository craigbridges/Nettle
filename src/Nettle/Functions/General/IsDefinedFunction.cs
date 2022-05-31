namespace Nettle.Functions.General
{
    using System;

    public sealed class IsDefinedFunction : FunctionBase
    {
        public IsDefinedFunction() : base()
        {
            DefineRequiredParameter("Path", "The path of the property to check.", typeof(string));
        }

        public override string Description => "Determines if a property has been defined.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var path = GetParameterValue<string>("Path", parameterValues);

            try
            {
                // Note:
                // Try to resolve the property value, if it doesn't exist an exception will be thrown.

                context.ResolvePropertyValue(path ?? String.Empty);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
