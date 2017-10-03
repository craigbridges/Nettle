namespace Nettle.Functions.String
{
    using Nettle.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represent a concatenate function implementation
    /// </summary>
    public sealed class ConcatFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ConcatFunction() 
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Concatenates a collection of values into a single string.";
            }
        }

        /// <summary>
        /// Concatenates every parameter value into a single string
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The concatenated text</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
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
        private string Concatenate
            (
                params object[] values
            )
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

                        foreach (var item in value as IEnumerable)
                        {
                            items.Add(item);
                        }

                        var segment = Concatenate
                        (
                            items.ToArray()
                        );

                        builder.Append(segment);
                    }
                    else
                    {
                        builder.Append
                        (
                            value.ToString()
                        );
                    }
                }
            }

            return builder.ToString();
        }
    }
}
