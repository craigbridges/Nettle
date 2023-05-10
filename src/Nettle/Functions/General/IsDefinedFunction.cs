namespace Nettle.Functions.General;

using System;
using System.Threading.Tasks;

public sealed class IsDefinedFunction : FunctionBase
{
    public IsDefinedFunction() : base()
    {
        DefineRequiredParameter("Path", "The path of the property to check.", typeof(string));
    }

    public override string Description => "Determines if a property has been defined.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var path = GetParameterValue<string>("Path", request);

        try
        {
            // Try to resolve the property value, if it doesn't exist an exception will be thrown.
            request.Context.ResolvePropertyValue(path ?? String.Empty);

            return Task.FromResult<object?>(true);
        }
        catch
        {
            return Task.FromResult<object?>(false);
        }
    }
}
