namespace Nettle.Functions.DateTime
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a convert date and time function implementation
    /// </summary>
    public sealed class ConvertTimeFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ConvertTimeFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Date",
                "The date and time to convert.",
                typeof(DateTime)
            );

            DefineRequiredParameter
            (
                "DestinationTimeZoneId",
                "The identifier of the destination time zone.",
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
                return "Converts a date to a specific time zone.";
            }
        }

        /// <summary>
        /// Converts the date time supplied to the local time
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

            var timeZoneId = GetParameterValue<string>
            (
                "DestinationTimeZoneId",
                parameterValues
            );

            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId
            (
                date,
                timeZoneId
            );
        }
    }
}
