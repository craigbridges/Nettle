namespace Nettle.Data.Functions;

using Nettle.Common.Serialization.Grid;
using Nettle.Functions;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Represents function for converting an object to an XML string
/// </summary>
public class ToXmlFunction : FunctionBase
{
    public ToXmlFunction()
    {
        DefineRequiredParameter("Object", "The object to convert.", typeof(object));
    }

    public override string Description => "Converts an object to an XML string.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var obj = GetParameterValue<object>("Object", request);
        var type = obj?.GetType() ?? typeof(object);

        var xml = String.Empty;

        if (type.IsDataGrid())
        {
            xml = ((IDataGrid)obj!).ToXml().Stringify();
        }
        else if (type == typeof(XmlDocument))
        {
            xml = ((XmlDocument)obj!).Stringify();
        }
        else
        {
            var serializer = new XmlSerializer(type);

            using (var sw = new StringWriter())
            {
                var settings = new XmlWriterSettings()
                {
                    Indent = true
                };

                using (var writer = XmlWriter.Create(sw, settings))
                {
                    serializer.Serialize(writer, obj);
                    xml = sw.ToString();
                }
            }
        }

        return Task.FromResult<object?>(xml);
    }
}
