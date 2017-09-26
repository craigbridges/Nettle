namespace Nettle.Functions.Math
{
    using Nettle.Compiler;
    using System;
    using System.Linq;

    /// <summary>
    /// Represents a largest number function implementation
    /// </summary>
    public sealed class LargestNumberFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public LargestNumberFunction() 
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Gets the largest number of a sequence.";
            }
        }

        /// <summary>
        /// LargestNumbers the parameter values supplied as a double
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The summed number</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var numbers = ConvertToNumbers(parameterValues);

            if (numbers.Length == 0)
            {
                throw new ArgumentException
                (
                    "The sequence does not contain any numbers."
                );
            }
            else
            {
                return numbers.OrderByDescending(a => a).First();
            }
        }
    }
}
