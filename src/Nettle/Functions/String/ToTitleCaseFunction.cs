namespace Nettle.Functions.String;

using System.Threading.Tasks;

public sealed class ToTitleCaseFunction : FunctionBase
{
    public ToTitleCaseFunction() : base()
    {
        DefineRequiredParameter("Text", "The text to convert.", typeof(string));
    }

    public override string Description => "Converts a string to title case.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var text = GetParameterValue<string>("Text", request);

        return Task.FromResult<object?>(text?.ToTitleCase());
    }
}
