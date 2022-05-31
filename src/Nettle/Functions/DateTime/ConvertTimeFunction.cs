namespace Nettle.Functions.DateTime;

using System;

public sealed class ConvertTimeFunction : FunctionBase
{
    public ConvertTimeFunction() : base()
    {
        DefineRequiredParameter("Date", "The date and time to convert.", typeof(DateTime));
        DefineRequiredParameter("DestinationTimeZoneId", "The identifier of the destination time zone.", typeof(string));
    }

    public override string Description => "Converts a date to a specific time zone.";

    protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
    {
        Validate.IsNotNull(context);

        var date = GetParameterValue<DateTime>("Date", parameterValues);
        var timeZoneId = GetParameterValue<string>("DestinationTimeZoneId", parameterValues);

        timeZoneId ??= TimeZoneInfo.Local.Id;

        return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, timeZoneId);
    }
}
