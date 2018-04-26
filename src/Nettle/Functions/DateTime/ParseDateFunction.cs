namespace Nettle.Functions.DateTime
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a parse date and time function implementation
    /// </summary>
    public sealed class ParseDateFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public ParseDateFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "RawValue",
                "The string value to parse.",
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
                return "Parses a string into a new date and time.";
            }
        }

        /// <summary>
        /// Gets the current date and time
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The date and time</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var rawValue = GetParameterValue<string>
            (
                "RawValue",
                parameterValues
            );

            return DateTime.Parse(rawValue);
        }
    }
}
