namespace Nettle.Functions.String
{
    public sealed class ConcatFunction : FunctionBase
    {
        public ConcatFunction() 
            : base()
        { }

        public override string Description => "Concatenates a collection of values into a single string.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var output = Concatenate(parameterValues);

            return output;
        }

        /// <summary>
        /// Concatenates an array of objects into a single string
        /// </summary>
        /// <param name="values">The values to concatenate</param>
        /// <returns>A string representing all the values</returns>
        /// <remarks>
        /// Values that are enumerable are recursively concatenated.
        /// </remarks>
        private string Concatenate(params object?[] values)
        {
            var builder = new StringBuilder();

            foreach (var value in values)
            {
                if (value != null)
                {
                    // Check if the parameter value is a collection
                    if (value.GetType().IsEnumerable(false))
                    {
                        var items = new List<object>();

                        foreach (var item in (IEnumerable)value)
                        {
                            items.Add(item);
                        }

                        var segment = Concatenate(items.ToArray());

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
}
