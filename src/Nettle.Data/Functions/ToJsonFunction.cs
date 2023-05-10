namespace Nettle.Data.Functions
{
    using Nettle.Common.Serialization.Grid;
    using Nettle.Functions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents function for converting an object to a JSON string
    /// </summary>
    public class ToJsonFunction : FunctionBase
    {
        public ToJsonFunction()
        {
            DefineRequiredParameter("Object", "The object to convert.", typeof(object));
        }

        public override string Description => "Converts an object to a JSON string.";

        protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
        {
            var obj = GetParameterValue<object>("Object", request);
            var type = obj?.GetType() ?? typeof(object);

            string json;

            if (obj == null)
            {
                json = String.Empty;
            }
            else if (type.IsDataGrid())
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

            return Task.FromResult<object?>(json);
        }
    }
}
