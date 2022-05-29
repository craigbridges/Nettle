namespace Nettle.Functions
{
    /// <summary>
    /// Represents POCO for holding the configuration details of a function parameter
    /// </summary>
    /// <param name="Name">The name of the parameter</param>
    /// <param name="DataType">The parameters data type</param>
    /// <remarks>
    /// The name must be a valid parameter name (i.e. alpha numeric and not contain spaces).
    /// </remarks>
    public record class FunctionParameterConfiguration(string Name, Type DataType)
    {
        /// <summary>
        /// Gets or sets a description for the parameter
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the parameters default value
        /// </summary>
        public object? DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a flag value used to determine if the parameter is optional
        /// </summary>
        public bool Optional { get; set; }
    }
}
