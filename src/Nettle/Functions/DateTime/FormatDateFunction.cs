namespace Nettle.Functions.DateTime;

using System;

public sealed class FormatDateFunction : FunctionBase
{
    public FormatDateFunction() : base()
    {
        DefineRequiredParameter("Date", "The date and time to format.", typeof(DateTime));
        DefineRequiredParameter("Format", "A standard or custom date and time format string.", typeof(string));
    }

    public override string Description => "Replicates the String.Format method.";

    protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
    {
        Validate.IsNotNull(context);

        var date = GetParameterValue<DateTime>("Date", parameterValues);
        var format = GetParameterValue<string>("Format", parameterValues);

        return date.ToString(format);
    }
}
