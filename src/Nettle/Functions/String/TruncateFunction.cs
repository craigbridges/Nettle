﻿namespace Nettle.Functions.String
{
    using Nettle.Compiler;

    /// <summary>
    /// Represent a truncate function implementation
    /// </summary>
    public sealed class TruncateFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public TruncateFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Text",
                "The text to truncate",
                typeof(string)
            );

            DefineRequiredParameter
            (
                "Length",
                "The texts maximum number of characters.",
                typeof(int)
            );
        }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Truncates a string to the length specified.";
            }
        }

        /// <summary>
        /// Truncates the string supplied to the length specified
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The truncated text</returns>
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

            var length = GetParameterValue<int>
            (
                "Length",
                parameterValues
            );

            return text.Truncate(length);
        }
    }
}
