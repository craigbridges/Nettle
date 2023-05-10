namespace Nettle.Functions.String;

using System.Threading.Tasks;

public sealed class JoinFunction : FunctionBase
{
    public JoinFunction() : base()
    {
        DefineRequiredParameter("Separator", "The string to use as a separator.", typeof(string));
    }

    public override string Description => "Joins an array of items, using the specified separator between each element.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var separator = GetParameterValue<string>("Separator", request);

        var values = request.ParameterValues.Skip(1).ToArray();
        var output = Join(separator, values);

        return Task.FromResult<object?>(output);
    }

    /// <summary>
    /// Joins an array of objects into a single string
    /// </summary>
    /// <param name="separator">The separator</param>
    /// <param name="values">The values to concatenate</param>
    /// <returns>A string representing all the values</returns>
    /// <remarks>
    /// Values that are enumerable are recursively joined.
    /// </remarks>
    private string Join(string? separator, params object?[] values)
    {
        var builder = new StringBuilder();

        foreach (var value in values)
        {
            if (value != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append(separator);
                }
                
                // Check if the parameter value is a collection
                if (value.GetType().IsEnumerable(false))
                {
                    var items = new List<object>();

                    foreach (var item in (IEnumerable)value)
                    {
                        items.Add(item);
                    }

                    var segment = Join(separator, items.ToArray());

                    builder.Append(segment);
                }
                else
                {
                    builder.Append(value.ToString());
                }
            }
        }

        return builder.ToString();
    }
}
