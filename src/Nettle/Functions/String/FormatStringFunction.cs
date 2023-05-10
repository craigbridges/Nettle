namespace Nettle.Functions.String;

using System;
using System.Threading.Tasks;

public sealed class FormatStringFunction : FunctionBase
{
    public FormatStringFunction() : base()
    {
        DefineRequiredParameter("Format", "The string to format.", typeof(string));
    }

    public override string Description =>
        "Replaces the format item in a specified string with the string " +
        "representation of a corresponding object in a specified array.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var format = GetParameterValue<string>("Format", request);
        var formatValues = request.ParameterValues.Skip(1);

        var formattedString = String.Format(format ?? String.Empty, formatValues.ToArray());

        return Task.FromResult<object?>(formattedString);
    }
}
