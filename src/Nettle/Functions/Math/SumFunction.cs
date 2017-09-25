namespace Nettle.Functions.Math
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a sum numbers function implementation
    /// </summary>
    public sealed class SumFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public SumFunction() 
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Computes the sum of a sequence of numeric values.";
            }
        }

        /// <summary>
        /// Sums the parameter values supplied as a double
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The sumed number</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var total = default(double);

            foreach (var value in parameterValues)
            {
                if (value == null || false == value.ToString().IsNumeric())
                {
                    throw new ArgumentException
                    (
                        "Only numeric values are supported."
                    );
                }

                total += Double.Parse
                (
                    value.ToString()
                );
            }

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
