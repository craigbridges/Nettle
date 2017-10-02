namespace Nettle.Functions.Math
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a random double function implementation
    /// </summary>
    public sealed class RandomDoubleFunction : FunctionBase
    {
        private static Random _random = new Random();

        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public RandomDoubleFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "MinValue",
                "The minimum value.",
                typeof(double)
            );

            DefineRequiredParameter
            (
                "MaxValue",
                "The maximum value.",
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
                return "Generates a random double between a range.";
            }
        }

        /// <summary>
        /// RandomDoubles two numbers
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

            var minValue = GetParameterValue<double>
            (
                "MinValue",
                parameterValues
            );

            var maxValue = GetParameterValue<double>
            (
                "MaxValue",
                parameterValues
            );

            var nextDouble = _random.NextDouble();

            var number =
            (
                nextDouble * (maxValue - minValue) + minValue
            );

            return number;
        }
    }
}
