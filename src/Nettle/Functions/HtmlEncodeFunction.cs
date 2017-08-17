namespace Nettle.Functions
{
    using System.Web;

    /// <summary>
    /// Represent a HTML encode function implementation
    /// </summary>
    public sealed class HtmlEncodeFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public HtmlEncodeFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Text",
                "The text to encode",
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
                return "HTML encodes text.";
            }
        }

        /// <summary>
        /// HTML encodes some text
        /// </summary>
        /// <param name="model">The template model</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The encoded text</returns>
        protected override object GenerateOutput
            (
                TemplateModel model,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(model);

            var text = GetParameterValue<string>
            (
                "Text",
                parameterValues
            );

            return HttpUtility.HtmlEncode
            (
                text
            );
        }
    }
}
