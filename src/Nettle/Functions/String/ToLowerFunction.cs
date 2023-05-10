namespace Nettle.Functions.String;

using System.Threading.Tasks;

public sealed class ToLowerFunction : FunctionBase
{
    public ToLowerFunction() : base()
    {
        DefineRequiredParameter("Text", "The text to convert.", typeof(string));
    }

    public override string Description => "Converts a string to lower case.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var text = GetParameterValue<string>("Text", request);

        return Task.FromResult<object?>(text?.ToLower());
    }
}
