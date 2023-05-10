namespace Nettle.Functions.DateTime;

using System;
using System.Threading.Tasks;

public sealed class FormatDateFunction : FunctionBase
{
    public FormatDateFunction() : base()
    {
        DefineRequiredParameter("Date", "The date and time to format.", typeof(DateTime));
        DefineRequiredParameter("Format", "A standard or custom date and time format string.", typeof(string));
    }

    public override string Description => "Replicates the String.Format method.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var date = GetParameterValue<DateTime>("Date", request);
        var format = GetParameterValue<string>("Format", request);

        return Task.FromResult<object?>(date.ToString(format));
    }
}
