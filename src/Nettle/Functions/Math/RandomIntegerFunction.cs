namespace Nettle.Functions.Math
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a random integer function implementation
    /// </summary>
    public sealed class RandomIntegerFunction : FunctionBase
    {
        private static Random _random = new Random();

        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public RandomIntegerFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "MinValue",
                "The minimum value.",
                typeof(int)
            );

            DefineRequiredParameter
            (
                "MaxValue",
                "The maximum value.",
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
                return "Generates a random integers between a range.";
            }
        }

        /// <summary>
        /// RandomIntegers two numbers
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

            var minValue = GetParameterValue<int>
            (
                "MinValue",
                parameterValues
            );

            var maxValue = GetParameterValue<int>
            (
                "MaxValue",
                parameterValues
            );
            
            return _random.Next
            (
                minValue,
                maxValue
            );
        }
    }
}
