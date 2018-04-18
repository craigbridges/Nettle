namespace Nettle.Functions.Conversion
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a convert to Int16 (long) function implementation
    /// </summary>
    public sealed class ToInt16Function : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ToInt16Function()
            : base()
        {
            DefineRequiredParameter
            (
                "Number",
                "The number",
                typeof(double)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Converts a double to an equivalent 16-bit signed integer.";
            }
        }

        /// <summary>
        /// Converts a number to an Int16 type
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

            var number = GetParameterValue<double>
            (
                "Number",
                parameterValues
            );

            return Convert.ToInt16(number);
        }
    }
}
