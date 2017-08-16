namespace Nettle.Functions
{
    using System;

    /// <summary>
    /// Represents POCO for holding the configuration details of a function parameter
    /// </summary>
    public class FunctionParameterConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the parameter
        /// </summary>
        /// <remarks>
        /// The name must be a valid parameter name (i.e. alpha numeric and not contain spaces)
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description for the parameter
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parameters data type
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// Gets or sets the parameters default value
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a flag value used to determine if the parameter is optional
        /// </summary>
        public bool Optional { get; set; }
    }
}
