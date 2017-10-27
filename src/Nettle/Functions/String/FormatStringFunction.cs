namespace Nettle.Functions.String
{
    using Nettle.Compiler;
    using System;
    using System.Linq;

    /// <summary>
    /// Represent a format string function implementation
    /// </summary>
    public sealed class FormatStringFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public FormatStringFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Format",
                "The string to format.",
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
                return "Converts the value of objects to" +
                       "strings based on the formats specified " +
                       "and inserts them into another string.";
            }
        }

        /// <summary>
        /// Formats the string using the values specified
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The formatted string</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var format = GetParameterValue<string>
            (
                "Format",
                parameterValues
            );
            
            var formatValues = parameterValues.Skip(1);

            var output = String.Format
            (
                format,
                formatValues.ToArray()
            );

            return output;
        }
    }
}
