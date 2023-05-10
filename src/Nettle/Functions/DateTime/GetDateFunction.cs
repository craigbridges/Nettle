namespace Nettle.Functions.DateTime;

using System;
using System.Threading.Tasks;

public sealed class GetDateFunction : FunctionBase
{
    public GetDateFunction() 
        : base()
    { }

    public override string Description => "Gets the current date and time.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var forceUtc = request.Context.IsFlagSet(TemplateFlag.UseUtc);
        var date = DateTime.UtcNow;

        if (false == forceUtc)
        {
            date = TimeZoneInfo.ConvertTime(date, NettleEngine.DefaultTimeZone);
        }

        return Task.FromResult<object?>(date);
    }
}
