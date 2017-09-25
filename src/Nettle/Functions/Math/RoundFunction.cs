namespace Nettle.Functions.Math
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a round number function implementation
    /// </summary>
    public sealed class RoundFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public RoundFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Number",
                "The number to round",
                typeof(double)
            );

            DefineRequiredParameter
            (
                "Decimals",
                "The number of decimal places.",
                typeof(int)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Rounds a number to a set number of decimal places.";
            }
        }

        /// <summary>
        /// Rounds a number to the decimal places specified
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

            var decimals = GetParameterValue<int>
            (
                "Decimals",
                parameterValues
            );

            return Math.Round(number, decimals);
        }
    }
}
