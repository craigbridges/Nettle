namespace Nettle.Functions
{
    /// <summary>
    /// Represents a request to execute a Nettle function
    /// </summary>
    public record class FunctionExecutionRequest
    {
        /// <summary>
        /// The current template context
        /// </summary>
        public required TemplateContext Context { get; init; }

        /// <summary>
        /// The parameter values to pass into the function
        /// </summary>
        public required object?[] ParameterValues { get; init; }
    }
}
