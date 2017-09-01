namespace Nettle.Data.Functions
{
    using Nettle.Compiler;
    using Nettle.Functions;
    using System;

    /// <summary>
    /// Represents function for generating an array of random numbers
    /// </summary>
    public class GenerateRandomNumbersFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public GenerateRandomNumbersFunction()
        {
            DefineOptionalParameter
            (
                "Size",
                "The array size",
                typeof(int),
                10
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Generates an array of random integers.";
            }
        }

        /// <summary>
        /// Generates an array of random integers
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The truncated text</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var size = GetParameterValue<int>
            (
                "Size",
                parameterValues
            );

            var data = new int[size];
            var rnd = new Random();

            for (var i = 0; i < size; i++)
            {
                data[i] = rnd.Next();
            }
            
            return data;
        }
    }
}
