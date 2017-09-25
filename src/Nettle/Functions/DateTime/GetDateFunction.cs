namespace Nettle.Functions.DateTime
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represent a get date and time function implementation
    /// </summary>
    public sealed class GetDateFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public GetDateFunction() 
            : base()
        { }

        /// <summary>
        /// Gets a description of the function
        /// </summary>
        public override string Description
        {
            get
            {
                return "Gets the current local date and time.";
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

            return DateTime.Now;
        }
    }
}
