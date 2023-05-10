namespace Nettle.Functions.String;

using System;
using System.Threading.Tasks;

public sealed class ReplaceFunction : FunctionBase
{
    public ReplaceFunction() : base()
    {
        DefineRequiredParameter("Text", "The original text", typeof(string));
        DefineRequiredParameter("OldValue", "The old value", typeof(string));
        DefineRequiredParameter("NewValue", "The new value", typeof(string));
    }

    public override string Description => "Replaces text in a string with other text.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var oldText = GetParameterValue<string>("Text", request);
        var oldValue = GetParameterValue<string>("OldValue", request);
        var newValue = GetParameterValue<string>("NewValue", request);

        var newText = oldText?.Replace(oldValue ?? String.Empty, newValue);

        return Task.FromResult<object?>(newText);
    }
}
