namespace Nettle.Functions.Core
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a pad right function implementation
    /// </summary>
    public sealed class PadRightFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public PadRightFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Text",
                "The text to pad",
                typeof(string)
            );

            DefineRequiredParameter
            (
                "TotalWidth",
                "The number of characters in the resulting string.",
                typeof(int)
            );

            DefineRequiredParameter
            (
                "PaddingChar",
                "The padding character",
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
                return "Right pads a string to the length specified.";
            }
        }

        /// <summary>
        /// Right pads the string supplied to the length specified
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The padded text</returns>
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

            var totalWidth = GetParameterValue<int>
            (
                "TotalWidth",
                parameterValues
            );

            var paddingText = GetParameterValue<string>
            (
                "PaddingChar",
                parameterValues
            );

            if (paddingText.Length > 1)
            {
                throw new ArgumentException
                (
                    "The padding char must be a single character."
                );
            }

            var paddingChar = paddingText.ToCharArray()[0];

            return text.PadRight
            (
                totalWidth,
                paddingChar
            );
        }
    }
}
