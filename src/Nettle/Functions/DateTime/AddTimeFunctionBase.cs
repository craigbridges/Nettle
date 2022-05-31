namespace Nettle.Functions.DateTime
{
    using System;

    /// <summary>
    /// Represent a base function for adding time to a date
    /// </summary>
    public abstract class AddTimeFunctionBase : FunctionBase
    {
        public AddTimeFunctionBase() : base()
        {
            DefineRequiredParameter("Date", "The date and time to adjust.", typeof(DateTime));
            DefineRequiredParameter("Value", "A number of whole and fractional time units.", typeof(double));
        }

        public override string Description => "Adds a unit of time to a DateTime value.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var date = GetParameterValue<DateTime>("Date", parameterValues);
            var value = GetParameterValue<double>("Value", parameterValues);

            return AddTime(date, value);
        }

        protected abstract DateTime AddTime(DateTime date, double value);
    }
}
