namespace Nettle.Functions.Conversion;

using System.Threading.Tasks;

/// <summary>
/// Represent a convert object to type function base class
/// </summary>
/// <typeparam name="TTo">The object type to convert to</typeparam>
public abstract class ConvertFunctionBase<TTo> : FunctionBase
{
    public ConvertFunctionBase() : base()
    {
        DefineRequiredParameter("Value", "The object value to convert.", typeof(object));
    }

    public override string Description => $"Converts an object to a {typeof(TTo).Name}.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var rawValue = GetParameterValue<object?>("Value", request);
        var converter = new GenericObjectToTypeConverter<TTo>();
        var convertedValue = converter.Convert(rawValue);

        return Task.FromResult<object?>(convertedValue);
    }
}
