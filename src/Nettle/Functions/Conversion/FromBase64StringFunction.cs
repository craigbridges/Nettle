namespace Nettle.Functions.Conversion
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a convert base-64 string to byte array function implementation
    /// </summary>
    public sealed class FromBase64StringFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public FromBase64StringFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Data",
                "The base-64 encoded string.",
                typeof(string)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Converts a base-64 encoded string to a byte array.";
            }
        }

        /// <summary>
        /// Converts a base-64 encoded string to a byte array
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

            var data = GetParameterValue<string>
            (
                "Data",
                parameterValues
            );

            return Convert.FromBase64String
            (
                data
            );
        }
    }
}
