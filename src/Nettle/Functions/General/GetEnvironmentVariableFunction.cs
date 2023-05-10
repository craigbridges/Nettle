namespace Nettle.Functions.General;

using System;
using System.Threading.Tasks;

public sealed class GetEnvironmentVariableFunction : FunctionBase
{
    public GetEnvironmentVariableFunction() : base()
    {
        DefineRequiredParameter("Name", "The name of the variable to get.", typeof(string));
    }

    public override string Description => "Retrieves the value of a Nettle environment variable.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var name = GetParameterValue<string>("Name", request);
        var value = NettleEnvironment.GetEnvironmentVariable(name ?? String.Empty);

        return Task.FromResult(value);
    }
}
