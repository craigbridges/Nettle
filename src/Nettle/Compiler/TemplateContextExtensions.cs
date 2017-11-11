namespace Nettle.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;

    /// <summary>
    /// Various extension methods for the template context
    /// </summary>
    internal static class TemplateContextExtensions
    {
        /// <summary>
        /// Generates debug information for a template context
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="renderTime">The render time</param>
        /// <returns>The debug information represented as a string</returns>
        public static string GenerateDebugInfo
            (
                this TemplateContext context,
                TimeSpan renderTime
            )
        {
            Validate.IsNotNull(context);

            var builder = new StringBuilder();

            var mainHeading = GenerateDebugHeading
            (
                "Debug Information"
            );

            var renderTimeFormatted = "{0} milliseconds".With
            (
                renderTime.Milliseconds
            );

            builder.Append(mainHeading);
            builder.Append("\r\n\r\n");

            builder.Append
            (
                "Render Time: {0}".With
                (
                    renderTimeFormatted
                )
            );

            // Generate property debug info
            var propertyHeading = GenerateDebugHeading
            (
                "Properties"
            );

            builder.Append("\r\n\r\n");
            builder.Append(propertyHeading);
            builder.Append("\r\n");

            foreach (var property in context.PropertyValues)
            {
                builder.Append
                (
                    "\r\n{0}: {1}".With
                    (
                        property.Key,
                        property.Value
                    )
                );
            }

            // Generate variable debug info
            var variableHeading = GenerateDebugHeading
            (
                "Variables"
            );

            builder.Append("\r\n\r\n");
            builder.Append(variableHeading);
            builder.Append("\r\n");

            foreach (var variable in context.Variables)
            {
                builder.Append
                (
                    "\r\n{0}: {1}".With
                    (
                        variable.Key,
                        variable.Value
                    )
                );
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generates a debug heading string
        /// </summary>
        /// <param name="text">The text for the heading</param>
        /// <returns>The heading generated</returns>
        private static string GenerateDebugHeading
            (
                string text
            )
        {
            var underline = new String
            (
                '-',
                text.Length
            );

            return "{0}\r\n{1}".With
            (
                text,
                underline
            );
        }
    }
}
