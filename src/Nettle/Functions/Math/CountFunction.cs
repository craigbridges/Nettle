namespace Nettle.Functions.Math
{
    using Nettle.Compiler;
    using System.Linq;
    using System.Collections;

    /// <summary>
    /// Represent a count function implementation
    /// </summary>
    public sealed class CountFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public CountFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Collection",
                "The collection to count",
                typeof(IEnumerable)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Counts the number of items in a collection.";
            }
        }

        /// <summary>
        /// Counts the number of items in the collection supplied
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The rounded number</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var collection = GetParameterValue<object>
            (
                "Collection",
                parameterValues
            );
            
            if (collection == null)
            {
                return 0;
            }
            else
            {
                var count = default(int);

                foreach (var item in collection as IEnumerable)
                {
                    count++;
                }

                return count;
            }
        }
    }
}
