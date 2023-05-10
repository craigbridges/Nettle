namespace Nettle.Functions.Math;

using System.Threading.Tasks;

public sealed class SmallestNumberFunction : FunctionBase
{
    public SmallestNumberFunction() 
        : base()
    { }

    public override string Description => "Gets the smallest number of a sequence.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var numbers = ConvertToNumbers(request);

        if (numbers.Length == 0)
        {
            throw new ArgumentException("The sequence does not contain any numbers.");
        }
        else
        {
            var smallestNumber = numbers.OrderBy(number => number).First();

            return Task.FromResult<object?>(smallestNumber);
        }
    }
}
