namespace Nettle.Functions
{
    using System;

    /// <summary>
    /// Represents a single function parameter
    /// </summary>
    public sealed class FunctionParameter
    {
        /// <summary>
        /// Constructs the function parameter with dependencies
        /// </summary>
        /// <param name="function">The function</param>
        /// <param name="configuration">The configuration details</param>
        public FunctionParameter
            (
                IFunction function,
                FunctionParameterConfiguration configuration
            )
        {
            Validate.IsNotNull(function);
            Validate.IsNotNull(configuration);

            this.Function = function;

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
        private void Configure
            (
                FunctionParameterConfiguration configuration
            )
        {
            Validate.IsNotNull(configuration);
            Validate.IsNotEmpty(configuration.Name);
            Validate.IsNotNull(configuration.DataType);

            var defaultValue = configuration.DefaultValue;

            if (defaultValue != null)
            {
                if (false == IsValidParameterValue(defaultValue))
                {
                    throw new ArgumentException
                    (
                        "The default value for '{0}' is not valid.".With
                        (
                            this.Name
                        )
                    );
                }
            }

            this.Name = configuration.Name;
            this.Description = configuration.Description;
            this.DataType = configuration.DataType;
            this.Optional = configuration.Optional;
            this.DefaultValue = defaultValue;
        }

        /// <summary>
        /// Gets the name of the parameter
        /// </summary>
        /// <remarks>
        /// The name must be a valid parameter name (i.e. alpha numeric and not contain spaces)
        /// </remarks>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a description for the parameter
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the parameters data type
        /// </summary>
        public Type DataType { get; private set; }

        /// <summary>
        /// Gets the parameters default value
        /// </summary>
        public object DefaultValue { get; private set; }

        /// <summary>
        /// Gets a flag value used to determine if the parameter is optional
        /// </summary>
        public bool Optional { get; private set; }

        /// <summary>
        /// Determines if the function parameter is required (i.e. it doesn't have a default value)
        /// </summary>
        /// <returns>True, if the parameter is required; otherwise false</returns>
        public bool IsRequired()
        {
            return false == this.Optional;
        }

        /// <summary>
        /// Determines if the function parameter is optional (i.e. it has a default value)
        /// </summary>
        /// <returns>True, if the parameter is optional; otherwise false</returns>
        public bool IsOptional()
        {
            return this.Optional;
        }

        /// <summary>
        /// Determines if the value specified is valid for the function parameter
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True, if the value is valid; otherwise false</returns>
        public bool IsValidParameterValue
            (
                object value
            )
        {
            if (value == null)
            {
                return this.DataType.IsNullable();
            }
            else
            {
                if (this.DataType == typeof(object))
                {
                    return true;
                }
                else if (value.GetType() == this.DataType)
                {
                    return true;
                }
                else
                {
                    return value.GetType().CanConvert
                    (
                        this.DataType,
                        value
                    );
                }
            }
        }
    }
}
