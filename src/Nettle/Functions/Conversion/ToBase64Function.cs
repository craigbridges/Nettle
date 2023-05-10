namespace Nettle.Functions.Conversion;

using System.Threading.Tasks;

/// <summary>
/// Represent a convert byte array to base-64 function implementation
/// </summary>
public sealed class ToBase64Function : FunctionBase
{
    public ToBase64Function() : base()
    {
        DefineRequiredParameter("Data", "The byte array data.", typeof(byte[]));
    }

    public override string Description => "Converts a byte array to a base-64 string.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var data = GetParameterValue<byte[]>("Data", request);
        var base64String = Convert.ToBase64String(data ?? Array.Empty<byte>());

        return Task.FromResult<object?>(base64String);
    }
}
