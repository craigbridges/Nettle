namespace Nettle.Functions.Math;

using System;
using System.Threading.Tasks;

public sealed class RoundFunction : FunctionBase
{
    public RoundFunction() : base()
    {
        DefineRequiredParameter("Number", "The number to round", typeof(double));
        DefineRequiredParameter("Decimals", "The number of decimal places.", typeof(int));
    }

    public override string Description => "Rounds a number to a set number of decimal places.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var number = GetParameterValue<double>("Number", request);
        var decimals = GetParameterValue<int>("Decimals", request);

        number = Math.Round(number, decimals);

        return Task.FromResult<object?>(number);
    }
}
