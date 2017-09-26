namespace Nettle.Functions.Math
{
    using Nettle.Compiler;
    using System;
    using System.Linq;

    /// <summary>
    /// Represents a smallest number function implementation
    /// </summary>
    public sealed class SmallestNumberFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public SmallestNumberFunction() 
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Gets the smallest number of a sequence.";
            }
        }

        /// <summary>
        /// SmallestNumbers the parameter values supplied as a double
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
                return numbers.OrderBy(a => a).First();
            }
        }
    }
}
