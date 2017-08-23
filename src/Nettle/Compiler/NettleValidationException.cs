namespace Nettle.Compiler.Parsing.Blocks
{
    using System;
    using System.Text;

    /// <summary>
    /// Represents a Nettle validation exception
    /// </summary>
    public class NettleValidationException : Exception
    {
        private TemplateValidationError[] _errors;

        internal NettleValidationException
            (
                string message
            )

            : base(message)
        { }

        internal NettleValidationException
            (
                string message,
                Exception innerException
            )

            : base(message, innerException)
        { }

        internal NettleValidationException
            (
                params TemplateValidationError[] errors
            )

            : base
            (
                BuildMessage(errors)
            )
        {
            _errors = errors;
        }

        /// <summary>
        /// Builds the exceptions error message from the errors specified
        /// </summary>
        /// <param name="errors">An array of validation errors</param>
        /// <returns>The message that was built</returns>
        private static string BuildMessage
            (
                params TemplateValidationError[] errors
            )
        {
            var builder = new StringBuilder();

            builder.AppendLine("One or more validation errors were found:");
            builder.AppendLine();

            foreach (var error in errors)
            {
                builder.AppendLine
                (
                    error.Message
                );
            }

            return builder.ToString();
        }
    }
}
