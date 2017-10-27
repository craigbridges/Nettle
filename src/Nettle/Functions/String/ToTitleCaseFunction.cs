namespace Nettle.Functions.String
{
    using Nettle.Compiler;

    /// <summary>
    /// Represents a 'to title case' string function implementation
    /// </summary>
    public sealed class ToTitleCaseFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ToTitleCaseFunction() 
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
                return "Converts a string to title case.";
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

            return text.ToTitleCase();
        }
    }
}
