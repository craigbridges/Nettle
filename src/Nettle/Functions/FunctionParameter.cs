namespace Nettle.Functions
{
    /// <summary>
    /// Represents a single function parameter
    /// </summary>
    public sealed class FunctionParameter
    {
        public FunctionParameter(IFunction function, FunctionParameterConfiguration configuration)
        {
            Validate.IsNotNull(function);
            Validate.IsNotNull(configuration);

            Function = function;

            Configure(configuration);
        }

        /// <summary>
        /// Gets a reference to the function
        /// </summary>
        public IFunction Function { get; private set; }

        /// <summary>
        /// Configures the function parameter using the details specified
        /// </summary>
        /// <param name="configuration">The configuration details</param>
        private void Configure(FunctionParameterConfiguration configuration)
        {
            Validate.IsNotNull(configuration);
            Validate.IsNotEmpty(configuration.Name);
            Validate.IsNotNull(configuration.DataType);

            var defaultValue = configuration.DefaultValue;

            Name = configuration.Name;
            Description = configuration.Description;
            DataType = configuration.DataType;
            Optional = configuration.Optional;
            DefaultValue = defaultValue;

            if (defaultValue != null)
            {
                var isValid = IsValidParameterValue(defaultValue);

                if (false == isValid)
                {
                    throw new ArgumentException($"The default value for '{Name}' is not valid.");
                }
            }
        }

        /// <summary>
        /// Gets the name of the parameter
        /// </summary>
        /// <remarks>
        /// The name must be a valid parameter name (i.e. alpha numeric and not contain spaces)
        /// </remarks>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets a description for the parameter
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Gets the parameters data type
        /// </summary>
        public Type DataType { get; private set; } = default!;

        /// <summary>
        /// Gets the parameters default value
        /// </summary>
        public object? DefaultValue { get; private set; }

        /// <summary>
        /// Gets a flag value used to determine if the parameter is optional
        /// </summary>
        public bool Optional { get; private set; }

        /// <summary>
        /// Determines if the function parameter is required (i.e. it doesn't have a default value)
        /// </summary>
        /// <returns>True, if the parameter is required; otherwise false</returns>
        public bool IsRequired() => false == Optional;

        /// <summary>
        /// Determines if the function parameter is optional (i.e. it has a default value)
        /// </summary>
        /// <returns>True, if the parameter is optional; otherwise false</returns>
        public bool IsOptional() => Optional;

        /// <summary>
        /// Determines if the value specified is valid for the function parameter
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True, if the value is valid; otherwise false</returns>
        public bool IsValidParameterValue(object? value)
        {
            if (value == null)
            {
                return DataType.IsNullable();
            }
            else
            {
                if (DataType == typeof(object))
                {
                    return true;
                }
                else if (value.GetType() == DataType)
                {
                    return true;
                }
                else
                {
                    return value.GetType().CanConvert(DataType, value);
                }
            }
        }
    }
}
