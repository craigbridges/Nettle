namespace Nettle.Functions.String;

using System.Threading.Tasks;

public sealed class TrimFunction : FunctionBase
{
    public TrimFunction() : base()
    {
        DefineRequiredParameter("Text", "The text to trim.", typeof(string));
    }

    public override string Description => " Trims the whitespace from both ends of the string.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var text = GetParameterValue<string>("Text", request);

        return Task.FromResult<object?>(text?.Trim());
    }
}
