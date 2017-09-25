namespace Nettle.Functions.Math
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a subtract numbers function implementation
    /// </summary>
    public sealed class SubtractFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public SubtractFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "NumberOne",
                "The first number.",
                typeof(double)
            );

            DefineRequiredParameter
            (
                "NumberTwo",
                "The second number.",
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
                return "Subtracts two numbers from each other.";
            }
        }

        /// <summary>
        /// Subtracts two numbers
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

            var number1 = GetParameterValue<double>
            (
                "NumberOne",
                parameterValues
            );

            var number2 = GetParameterValue<double>
            (
                "NumberTwo",
                parameterValues
            );

            var total = (number1 - number2);

            if (total.IsWholeNumber())
            {
                return Convert.ToInt64(total);
            }
            else
            {
                return total;
            }
        }
    }
}
