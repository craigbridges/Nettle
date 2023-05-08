namespace Nettle
{    
    /// <summary>
    /// Represents the state of the Nettle environment
    /// </summary>
    public static class NettleEnvironment
    {
        private static readonly Dictionary<string, object?> _variables = InitializeVariables();

        /// <summary>
        /// Initializes the environment variables dictionary
        /// </summary>
        /// <returns>A new dictionary with system environment variables</returns>
        private static Dictionary<string, object?> InitializeVariables()
        {
            var variables = new Dictionary<string, object?>();

            foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
            {
                variables.Add(entry.Key.ToString()!, entry.Value);
            }

            return variables;
        }

        /// <summary>
        /// Retrieves the value of a Nettle environment variable
        /// </summary>
        /// <param name="name">The variable name</param>
        public static object? GetEnvironmentVariable(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("The variable name must be a non empty string.");
            }

            return _variables.TryGetValue(name, out object? value) ? value : null;
        }

        /// <summary>
        /// Sets the value of a Nettle environment variable
        /// </summary>
        /// <param name="name">The variable name</param>
        /// <param name="value">The value to set</param>
        public static void SetEnvironmentVariable(string name, object? value)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("The variable name must be a non empty string.");
            }

            _variables[name] = value;
        }
    }
}
