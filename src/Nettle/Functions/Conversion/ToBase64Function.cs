namespace Nettle.Functions.Conversion
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a convert byte array to base-64 function implementation
    /// </summary>
    public sealed class ToBase64Function : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ToBase64Function() 
            : base()
        {
            DefineRequiredParameter
            (
                "Data",
                "The byte array data.",
                typeof(byte[])
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Converts a byte array to a base-64 string.";
            }
        }

        /// <summary>
        /// Converts a byte array to a base-64 encoded string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The rounded number</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var data = GetParameterValue<byte[]>
            (
                "Data",
                parameterValues
            );

            return Convert.ToBase64String
            (
                data
            );
        }
    }
}
