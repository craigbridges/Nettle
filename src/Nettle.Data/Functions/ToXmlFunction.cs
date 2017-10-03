namespace Nettle.Data.Functions
{
    using Nettle.Common.Serialization.Grid;
    using Nettle.Compiler;
    using Nettle.Functions;
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents function for converting an object to an XML string
    /// </summary>
    public class ToXmlFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ToXmlFunction()
        {
            DefineRequiredParameter
            (
                "Object",
                "The object to convert.",
                typeof(object)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Converts an object to an XML string.";
            }
        }

        /// <summary>
        /// Converts an object to an XML string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The data grid</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var obj = GetParameterValue<object>
            (
                "Object",
                parameterValues
            );

            var xml = String.Empty;
            var type = obj.GetType();

            if (type.IsDataGrid())
            {
                xml = ((IDataGrid)obj).ToXml().Stringify();
            }
            else if (type == typeof(XmlDocument))
            {
                xml = ((XmlDocument)obj).Stringify();
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

            return xml;
        }
    }
}
