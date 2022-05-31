﻿namespace Nettle.Functions.Conversion
{
    /// <summary>
    /// Represent a convert byte array to base-64 function implementation
    /// </summary>
    public sealed class ToBase64Function : FunctionBase
    {
        public ToBase64Function() : base()
        {
            DefineRequiredParameter("Data", "The byte array data.", typeof(byte[]));
        }

        public override string Description => "Converts a byte array to a base-64 string.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var data = GetParameterValue<byte[]>("Data", parameterValues);

            return Convert.ToBase64String(data ?? Array.Empty<byte>());
        }
    }
}
