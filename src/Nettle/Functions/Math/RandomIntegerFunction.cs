namespace Nettle.Functions.Math;

using System.Threading.Tasks;

public sealed class RandomIntegerFunction : FunctionBase
{
    private static readonly Random _random = new();

    public RandomIntegerFunction() : base()
    {
        DefineRequiredParameter("MinValue", "The minimum value.", typeof(int));
        DefineRequiredParameter("MaxValue", "The maximum value.", typeof(int));
    }

    public override string Description => "Generates a random integer between a range.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var minValue = GetParameterValue<int>("MinValue", request);
        var maxValue = GetParameterValue<int>("MaxValue", request);

        var randomNumber = _random.Next(minValue, maxValue);

        return Task.FromResult<object?>(randomNumber);
    }
}
