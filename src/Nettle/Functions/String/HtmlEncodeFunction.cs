namespace Nettle.Functions.String;

using System;
using System.Threading.Tasks;
using System.Web;

public sealed class HtmlEncodeFunction : FunctionBase
{
    public HtmlEncodeFunction() : base()
    {
        DefineRequiredParameter("Text", "The text to encode.", typeof(string));
    }

    public override string Description => "HTML encodes text.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var text = GetParameterValue<string>("Text", request);
        var encodedString = HttpUtility.HtmlEncode(text ?? String.Empty);

        return Task.FromResult<object?>(encodedString);
    }
}
