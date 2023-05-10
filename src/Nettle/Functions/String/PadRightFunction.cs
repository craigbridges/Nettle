namespace Nettle.Functions.String;

using System.Threading.Tasks;

public sealed class PadRightFunction : FunctionBase
{
    public PadRightFunction() : base()
    {
        DefineRequiredParameter("Text", "The text to pad", typeof(string));
        DefineRequiredParameter("TotalWidth", "The number of characters in the resulting string.", typeof(int));
        DefineRequiredParameter("PaddingChar", "The padding character", typeof(string));
    }

    public override string Description => "Right pads a string to the length specified.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var text = GetParameterValue<string>("Text", request);
        var totalWidth = GetParameterValue<int>("TotalWidth", request);
        var paddingText = GetParameterValue<string>("PaddingChar", request);

        if (paddingText?.Length > 1)
        {
            throw new ArgumentException("The padding char must be a single character.");
        }

        var paddingChar = paddingText?.ToCharArray()[0] ?? ' ';
        var paddedString = text?.PadRight(totalWidth, paddingChar);

        return Task.FromResult<object?>(paddedString);
    }
}
