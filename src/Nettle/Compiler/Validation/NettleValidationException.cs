namespace Nettle.Compiler.Validation
{
    /// <summary>
    /// Represents a Nettle validation exception
    /// </summary>
    [Serializable]
    public class NettleValidationException : Exception
    {
        internal NettleValidationException(string message)
            : base(message)
        { }

        internal NettleValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        internal NettleValidationException(params TemplateValidationError[] errors)
            : base(BuildMessage(errors))
        {
            Errors = errors;
        }

        /// <summary>
        /// Gets the errors that were detected
        /// </summary>
        internal TemplateValidationError[] Errors { get; } = Array.Empty<TemplateValidationError>();

        /// <summary>
        /// Builds the exceptions error message from the errors specified
        /// </summary>
        /// <param name="errors">An array of validation errors</param>
        /// <returns>The message that was built</returns>
        private static string BuildMessage(params TemplateValidationError[] errors)
        {
            var builder = new StringBuilder();

            builder.AppendLine("One or more validation errors were found:");
            builder.AppendLine();

            foreach (var error in errors)
            {
                builder.AppendLine(error.Message);
            }

            return builder.ToString();
        }
    }
}
