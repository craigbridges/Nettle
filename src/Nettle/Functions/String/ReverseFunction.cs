namespace Nettle.Functions.String
{
    using Nettle.Compiler;
    using System.Linq;

    /// <summary>
    /// Represents a reverse string function implementation
    /// </summary>
    public sealed class ReverseFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ReverseFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Text",
                "The text to reverse.",
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
                return "Reverses the order of a string.";
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

            return new string
            (
                text.Reverse().ToArray()
            );
        }
    }
}
