namespace Nettle.Functions.Math;

using System.Threading.Tasks;

public sealed class RandomDoubleFunction : FunctionBase
{
    private static readonly Random _random = new();

    public RandomDoubleFunction() : base()
    {
        DefineRequiredParameter("MinValue", "The minimum value.", typeof(double));
        DefineRequiredParameter("MaxValue", "The maximum value.", typeof(double));
    }

    public override string Description => "Generates a random double between a range.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var minValue = GetParameterValue<double>("MinValue", request);
        var maxValue = GetParameterValue<double>("MaxValue", request);

        var nextDouble = _random.NextDouble();
        var number = (nextDouble * (maxValue - minValue) + minValue);

        return Task.FromResult<object?>(number);
    }
}
