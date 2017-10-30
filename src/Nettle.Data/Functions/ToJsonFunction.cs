namespace Nettle.Data.Functions
{
    using Nettle.Common.Serialization.Grid;
    using Nettle.Compiler;
    using Nettle.Functions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;

    /// <summary>
    /// Represents function for converting an object to a JSON string
    /// </summary>
    public class ToJsonFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ToJsonFunction()
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
                return "Converts an object to a JSON string.";
            }
        }

        /// <summary>
        /// Converts an object to a JSON string
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

            var json = String.Empty;
            var type = obj.GetType();

            if (type.IsDataGrid())
            {
                json = ((IDataGrid)obj).ToJson();
            }
            else if (type == typeof(JObject))
            {
                json = ((JObject)obj).ToString();
            }
            else if (type == typeof(JArray))
            {
                json = ((JArray)obj).ToString();
            }
            else
            {
                json = JsonConvert.SerializeObject(obj);
            }

            return json;
        }
    }
}
