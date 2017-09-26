namespace Nettle.Functions.Math
{
    using Nettle.Compiler;
    using System.Linq;

    /// <summary>
    /// Represent a average numbers function implementation
    /// </summary>
    public sealed class AverageFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public AverageFunction() 
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Computes the average from a sequence of numeric values.";
            }
        }

        /// <summary>
        /// Averages the parameter values supplied as a double
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
            
            return numbers.Average();
        }
    }
}
