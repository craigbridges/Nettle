namespace Nettle.Functions.DateTime
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a format date and time function implementation
    /// </summary>
    public sealed class ToLocalTimeFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ToLocalTimeFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Date",
                "The date and time to convert.",
                typeof(DateTime)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Converts the value of a date to local time.";
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
            
            return date.ToLocalTime();
        }
    }
}
