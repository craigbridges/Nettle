namespace Nettle.Functions.General
{
    using Nettle.Compiler;

    /// <summary>
    /// Represents an function that determines if a property is defined
    /// </summary>
    public sealed class IsDefinedFunction : FunctionBase
    {
        /// <summary>
        /// Constructs the function by defining the parameters
        /// </summary>
        public IsDefinedFunction() 
            : base()
        {
            DefineRequiredParameter
            (
                "Path",
                "The path of the property to check.",
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
                return "Determines if a property has been defined.";
            }
        }

        /// <summary>
        /// Determines if the path is resolvable
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>True, if the path exists; otherwise false</returns>
        protected override object GenerateOutput
            (
                TemplateContext context,
                params object[] parameterValues
            )
        {
            Validate.IsNotNull(context);

            var path = GetParameterValue<string>
            (
                "Path",
                parameterValues
            );

            try
            {
                // Note:
                // Try to resolve the property value, if it doesn't 
                // exist an exception will be thrown.

                context.ResolvePropertyValue(path);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
