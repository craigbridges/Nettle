namespace Nettle.Data.Functions;

using CsvHelper;
using Nettle.Common.Serialization.Grid;
using Nettle.Compiler;
using Nettle.Functions;
using System;
using System.Collections;
using System.Globalization;
using System.IO;

/// <summary>
/// Represents function for converting an object to a CSV string
/// </summary>
public class ToCsvFunction : FunctionBase
{
    /// <summary>
    /// Constructs the function by defining the parameters
    /// </summary>
    public ToCsvFunction()
    {
        DefineRequiredParameter("Object", "The object to convert.", typeof(object));
    }

    /// <summary>
    /// Gets a description of the function
    /// </summary>
    public override string Description => "Converts an object to a CSV string.";

    /// <summary>
    /// Converts an object to a CSV string
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="parameterValues">The parameter values</param>
    /// <returns>The data grid</returns>
    protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
    {
        Nettle.Validate.IsNotNull(context);

        var obj = GetParameterValue<object>("Object", parameterValues);
        var type = obj?.GetType() ?? typeof(object);
        var csv = String.Empty;

        if (type.IsDataGrid())
        {
            csv = ((IDataGrid)obj!).ToCsv();
        }
        else
        {
            using (var textWriter = new StringWriter())
            {
                using (var csvWriter = new CsvWriter(textWriter, CultureInfo.CurrentCulture))
                {
                    if (type.IsEnumerable())
                    {
                        csvWriter.WriteRecords((IEnumerable)obj!);
                    }
                    else
                    {
                        csvWriter.WriteRecords(new object[] { obj ?? new { } });
                    }
                }

                csv = textWriter.ToString();
            }
        }

        return csv;
    }
}
