namespace Nettle.Functions.DateTime;

using System;
using System.Threading.Tasks;

public sealed class ParseDateFunction : FunctionBase
{
    public ParseDateFunction() : base()
    {
        DefineRequiredParameter("Value", "The string value to parse.", typeof(string));
    }

    public override string Description => "Parses a string into a new date and time.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var rawValue = GetParameterValue<string>("Value", request);
        var date = DateTime.Parse(rawValue ?? String.Empty);

        return Task.FromResult<object?>(date);
    }
}
