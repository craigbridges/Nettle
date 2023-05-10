namespace Nettle.Functions.String;

using System.Threading.Tasks;

public sealed class ToUpperFunction : FunctionBase
{
    public ToUpperFunction() : base()
    {
        DefineRequiredParameter("Text", "The text to convert.", typeof(string));
    }

    public override string Description => "Converts a string to upper case.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var text = GetParameterValue<string>("Text", request);

        return Task.FromResult<object?>(text?.ToUpper());
    }
}
