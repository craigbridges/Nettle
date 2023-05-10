namespace Nettle.Functions.String;

using System.Threading.Tasks;

public sealed class ReverseFunction : FunctionBase
{
    public ReverseFunction() : base()
    {
        DefineRequiredParameter("Text", "The text to reverse.", typeof(string));
    }

    public override string Description => "Reverses the order of a string.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var originalText = GetParameterValue<string>("Text", request);
        var reversedText = new string(originalText?.Reverse().ToArray());

        return Task.FromResult<object?>(reversedText);
    }
}
