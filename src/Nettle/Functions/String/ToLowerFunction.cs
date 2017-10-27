namespace Nettle.Functions.String
{
    using Nettle.Compiler;

    /// <summary>
    /// Represents a 'to lower' string function implementation
    /// </summary>
    public sealed class ToLowerFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ToLowerFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Text",
                "The text to convert.",
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
                return "Converts a string to lower case.";
            }
        }

        /// <summary>
        /// HTML encodes some text
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The encoded text</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var text = GetParameterValue<string>
            (
                "Text",
                parameterValues
            );

            return text.ToLower();
        }
    }
}
