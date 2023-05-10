namespace Nettle.Functions.Math;

using System.Linq;
using System.Threading.Tasks;

public sealed class AverageFunction : FunctionBase
{
    public AverageFunction() 
        : base()
    { }

    public override string Description => "Computes the average from a sequence of numeric values.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var numbers = ConvertToNumbers(request.ParameterValues);
        var average = numbers.Average();

        return Task.FromResult<object?>(average);
    }
}
