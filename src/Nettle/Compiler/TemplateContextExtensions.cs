namespace Nettle.Compiler
{
    using System;
    using System.Globalization;
    using System.Text;
    
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
            
            var renderTimeFormatted = "{0} milliseconds".With
            (
                renderTime.Milliseconds
            );

            builder.Append
            (
                GenerateDebugHeading
                (
                    "Debug Information"
                )
            );
            
            builder.Append
            (
                GenerateDebugDetail
                (
                    "Render Time",
                    renderTimeFormatted
                )
            );

            builder.Append
            (
                GenerateDebugDetail
                (
                    "Current Culture Name",
                    CultureInfo.CurrentCulture.Name
                )
            );

            builder.Append
            (
                GenerateDebugDetail
                (
                    "Current Culture Description",
                    CultureInfo.CurrentCulture.DisplayName
                )
            );

            builder.Append
            (
                GenerateDebugDetail
                (
                    "Default Time Zone ID",
                    NettleEngine.DefaultTimeZone.Id
                )
            );

            builder.Append
            (
                GenerateDebugDetail
                (
                    "Default Time Zone Name",
                    NettleEngine.DefaultTimeZone.DisplayName
                )
            );
                        
            // Generate the property debug info
            builder.Append("\r\n\r\n");

            builder.Append
            (
                GenerateDebugHeading
                (
                    "Properties"
                )
            );
            
            foreach (var property in context.PropertyValues)
            {
                builder.Append
                (
                    GenerateDebugDetail
                    (
                        property.Key,
                        property.Value
                    )
                );
            }

            // Generate the variable debug info
            builder.Append("\r\n\r\n");

            builder.Append
            (
                GenerateDebugHeading
                (
                    "Variables"
                )
            );

            foreach (var variable in context.Variables)
            {
                builder.Append
                (
                    GenerateDebugDetail
                    (
                        variable.Key,
                        variable.Value
                    )
                );
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generates a debug detail string
        /// </summary>
        /// <param name="label">The label</param>
        /// <param name="value">The value</param>
        /// <returns>The detail generated</returns>
        private static string GenerateDebugDetail
            (
                string label,
                object value
            )
        {
            return "\r\n{0}: {1}".With
            (
                label,
                value
            );
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

            return "{0}\r\n{1}\r\n".With
            (
                text,
                underline
            );
        }
    }
}
