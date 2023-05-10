namespace Nettle.Functions.Math;

using System.Threading.Tasks;

public sealed class SumFunction : FunctionBase
{
    public SumFunction() 
        : base()
    { }

    public override string Description => "Computes the sum of a sequence of numeric values.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var numbers = ConvertToNumbers(request);
        var total = numbers.Sum();

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
