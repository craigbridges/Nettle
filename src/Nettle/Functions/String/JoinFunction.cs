namespace Nettle.Functions.String
{
    using Nettle.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represent a join strings function implementation
    /// </summary>
    public sealed class JoinFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public JoinFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Separator",
                "The string to use as a separator.",
                typeof(string)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Joins an array of items, using the specified separator between each element.";
            }
        }

        /// <summary>
        /// Joins every parameter value into a single string
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

            var separator = GetParameterValue<string>
            (
                "Separator",
                parameterValues
            );

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
        private string Join
            (
                string separator,
                params object[] values
            )
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

                        foreach (var item in value as IEnumerable)
                        {
                            items.Add(item);
                        }

                        var segment = Join
                        (
                            separator,
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
