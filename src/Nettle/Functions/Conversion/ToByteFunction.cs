namespace Nettle.Functions.Conversion
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a convert object to byte function implementation
    /// </summary>
    public sealed class ToByteFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ToByteFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Value",
                "The object value to convert.",
                typeof(object)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Converts an object to a byte.";
            }
        }

        /// <summary>
        /// Converts the object to the conversion type
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

            var value = GetParameterValue<object>
            (
                "Value",
                parameterValues
            );

            return Convert.ToByte
            (
                value
            );
        }
    }
}
