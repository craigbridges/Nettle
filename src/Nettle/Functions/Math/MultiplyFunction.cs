namespace Nettle.Functions.Math;

using System.Threading.Tasks;

public sealed class MultiplyFunction : FunctionBase
{
    public MultiplyFunction() : base()
    {
        DefineRequiredParameter("NumberOne", "The first number.", typeof(double));
        DefineRequiredParameter("NumberTwo", "The second number.", typeof(double));
    }

    public override string Description => "Multiples two numbers together.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var number1 = GetParameterValue<double>("NumberOne", request);
        var number2 = GetParameterValue<double>("NumberTwo", request);

        var total = (number1 * number2);

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
