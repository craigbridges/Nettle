namespace Nettle.Functions.String;

public sealed class PadRightFunction : FunctionBase
{
    public PadRightFunction() : base()
    {
        DefineRequiredParameter("Text", "The text to pad", typeof(string));
        DefineRequiredParameter("TotalWidth", "The number of characters in the resulting string.", typeof(int));
        DefineRequiredParameter("PaddingChar", "The padding character", typeof(string));
    }

    public override string Description => "Right pads a string to the length specified.";

    protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
    {
        Validate.IsNotNull(context);

        var text = GetParameterValue<string>("Text", parameterValues);
        var totalWidth = GetParameterValue<int>("TotalWidth", parameterValues);
        var paddingText = GetParameterValue<string>("PaddingChar", parameterValues);

        if (paddingText?.Length > 1)
        {
            throw new ArgumentException("The padding char must be a single character.");
        }

        var paddingChar = paddingText?.ToCharArray()[0] ?? ' ';

        return text?.PadRight(totalWidth, paddingChar);
    }
}
