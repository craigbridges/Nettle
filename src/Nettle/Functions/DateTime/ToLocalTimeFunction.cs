namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class ToLocalTimeFunction : FunctionBase
    {
        public ToLocalTimeFunction() : base()
        {
            DefineRequiredParameter("Date", "The date and time to convert.", typeof(DateTime));
        }

        public override string Description => "Converts the value of a date to local time.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var date = GetParameterValue<DateTime>("Date", parameterValues);

            return TimeZoneInfo.ConvertTime(date, NettleEngine.DefaultTimeZone);
        }
    }
}
