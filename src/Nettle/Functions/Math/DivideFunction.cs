namespace Nettle.Functions.Math;

using System;
using System.Threading.Tasks;

public sealed class DivideFunction : FunctionBase
{
    public DivideFunction() : base()
    {
        DefineRequiredParameter("NumberOne", "The first number.", typeof(double));
        DefineRequiredParameter("NumberTwo", "The second number.", typeof(double));
    }

    public override string Description => "Divides two numbers.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var number1 = GetParameterValue<double>("NumberOne", request);
        var number2 = GetParameterValue<double>("NumberTwo", request);

        var total = (number1 / number2);

        if (total.IsWholeNumber())
        {
            return Task.FromResult<object?>(Convert.ToInt64(total));
        }
        else
        {
            return Task.FromResult<object?>(total);
        }
    }
}
