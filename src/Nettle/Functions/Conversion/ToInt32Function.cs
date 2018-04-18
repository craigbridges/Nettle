namespace Nettle.Functions.Conversion
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a convert to Int32 (long) function implementation
    /// </summary>
    public sealed class ToInt32Function : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ToInt32Function() 
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
                return "Converts a double to an equivalent 64-bit signed integer.";
            }
        }

        /// <summary>
        /// Converts a number to an Int32 type
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

            return Convert.ToInt32(number);
        }
    }
}
