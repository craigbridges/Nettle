namespace Nettle.Data.Functions;

using CsvHelper;
using Nettle.Common.Serialization.Grid;
using Nettle.Functions;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Represents function for converting an object to a CSV string
/// </summary>
public class ToCsvFunction : FunctionBase
{
    public ToCsvFunction()
    {
        DefineRequiredParameter("Object", "The object to convert.", typeof(object));
    }

    public override string Description => "Converts an object to a CSV string.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var obj = GetParameterValue<object>("Object", request);
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

        return Task.FromResult<object?>(csv);
    }
}
