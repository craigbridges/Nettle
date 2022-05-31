namespace Nettle.Functions.Math
{
    public sealed class CountFunction : FunctionBase
    {
        public CountFunction() : base()
        {
            DefineRequiredParameter("Collection", "The collection to count", typeof(IEnumerable));
        }

        public override string Description => "Counts the number of items in a collection.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var collection = GetParameterValue<object>("Collection", parameterValues);
            
            if (collection == null)
            {
                return 0;
            }
            else
            {
                var count = default(int);

                foreach (var item in (IEnumerable)collection)
                {
                    count++;
                }

                return count;
            }
        }
    }
}
