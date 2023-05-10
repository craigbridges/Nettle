namespace Nettle.Functions.Math;

using System.Threading.Tasks;

public sealed class CountFunction : FunctionBase
{
    public CountFunction() : base()
    {
        DefineRequiredParameter("Collection", "The collection to count", typeof(IEnumerable));
    }

    public override string Description => "Counts the number of items in a collection.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var collection = GetParameterValue<object>("Collection", request);

        int count = default;

        if (collection != null)
        {
            foreach (var item in (IEnumerable)collection)
            {
                count++;
            }
        }

        return Task.FromResult<object?>(count);
    }
}
