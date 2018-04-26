namespace Nettle.Functions.DateTime
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent an add milliseconds to date and time function implementation
    /// </summary>
    public sealed class AddMillisecondsFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public AddMillisecondsFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Date",
                "The date and time to adjust.",
                typeof(DateTime)
            );

            DefineRequiredParameter
            (
                "Value",
                "A number of whole and fractional milliseconds.",
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
                return "Replicates the DateTime.AddMilliseconds method.";
            }
        }

        /// <summary>
        /// Formats the date time supplied to the format specified
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The formatted date</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var date = GetParameterValue<DateTime>
            (
                "Date",
                parameterValues
            );

            var value = GetParameterValue<double>
            (
                "Value",
                parameterValues
            );

            return date.AddMilliseconds(value);
        }
    }
}
