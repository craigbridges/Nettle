namespace Nettle.Functions.DateTime
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a format date and time function implementation
    /// </summary>
    public sealed class FormatDateFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public FormatDateFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Date",
                "The date and time to format.",
                typeof(DateTime)
            );

            DefineRequiredParameter
            (
                "Format",
                "A standard or custom date and time format string.",
                typeof(string)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Replicates the String.Format method.";
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

            var format = GetParameterValue<string>
            (
                "Format",
                parameterValues
            );

            return date.ToString(format);
        }
    }
}
