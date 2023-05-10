namespace Nettle.Functions.String;

using System.Threading.Tasks;

public sealed class TruncateFunction : FunctionBase
{
    public TruncateFunction() : base()
    {
        DefineRequiredParameter("Text", "The text to truncate", typeof(string));
        DefineRequiredParameter("Length", "The texts maximum number of characters.", typeof(int));
    }

    public override string Description => "Truncates a string to the length specified.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var originalText = GetParameterValue<string>("Text", request);
        var length = GetParameterValue<int>("Length", request);

        var truncatedText = originalText?.Truncate(length);

        return Task.FromResult<object?>(truncatedText);
    }
}
