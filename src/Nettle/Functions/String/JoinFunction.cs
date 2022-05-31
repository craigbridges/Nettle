namespace Nettle.Functions.String;

public sealed class JoinFunction : FunctionBase
{
    public JoinFunction() : base()
    {
        DefineRequiredParameter("Separator", "The string to use as a separator.", typeof(string));
    }

    public override string Description => "Joins an array of items, using the specified separator between each element.";

    protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
    {
        Validate.IsNotNull(context);

        var separator = GetParameterValue<string>("Separator", parameterValues);

        var values = parameterValues.Skip(1).ToArray();
        var output = Join(separator, values);

        return output;
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
