namespace Nettle.Functions.DateTime;

using System;
using System.Threading.Tasks;

public sealed class ToLocalTimeFunction : FunctionBase
{
    public ToLocalTimeFunction() : base()
    {
        DefineRequiredParameter("Date", "The date and time to convert.", typeof(DateTime));
    }

    public override string Description => "Converts a date to local time.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var originalDate = GetParameterValue<DateTime>("Date", request);
        var convertedDate = TimeZoneInfo.ConvertTime(originalDate, NettleEngine.DefaultTimeZone);

        return Task.FromResult<object?>(convertedDate);
    }
}
