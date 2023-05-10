namespace Nettle.Functions.Math;

using System.Threading.Tasks;

public sealed class LargestNumberFunction : FunctionBase
{
    public LargestNumberFunction() 
        : base()
    { }

    public override string Description => "Gets the largest number of a sequence.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var numbers = ConvertToNumbers(request.ParameterValues);

        if (numbers.Length == 0)
        {
            throw new ArgumentException("The sequence does not contain any numbers.");
        }
        else
        {
            var largestNumber = numbers.OrderByDescending(number => number).First();

            return Task.FromResult<object?>(largestNumber);
        }
    }
}
