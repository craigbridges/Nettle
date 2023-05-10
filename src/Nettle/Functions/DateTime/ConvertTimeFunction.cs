namespace Nettle.Functions.DateTime;

using System;
using System.Threading.Tasks;

public sealed class ConvertTimeFunction : FunctionBase
{
    public ConvertTimeFunction() : base()
    {
        DefineRequiredParameter("Date", "The date and time to convert.", typeof(DateTime));
        DefineRequiredParameter("DestinationTimeZoneId", "The identifier of the destination time zone.", typeof(string));
    }

    public override string Description => "Converts a date to a specific time zone.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var date = GetParameterValue<DateTime>("Date", request);
        var timeZoneId = GetParameterValue<string>("DestinationTimeZoneId", request) ?? TimeZoneInfo.Local.Id;
        var convertedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, timeZoneId);

        return Task.FromResult<object?>(convertedDate);
    }
}
