namespace Nettle.Functions.Conversion;

using System;
using System.Threading.Tasks;

/// <summary>
/// Represent a convert base-64 string to byte array function implementation
/// </summary>
public sealed class FromBase64StringFunction : FunctionBase
{
    public FromBase64StringFunction() : base()
    {
        DefineRequiredParameter("Base64String", "The base-64 encoded string.", typeof(string));
    }

    public override string Description => "Converts a base-64 encoded string to a byte array.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var base64String = GetParameterValue<string>("Base64String", request);
        var data = Convert.FromBase64String(base64String ?? String.Empty);

        return Task.FromResult<object?>(data);
    }
}
