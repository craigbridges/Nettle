namespace Nettle.Compiler;

using System.Globalization;

internal static class TemplateContextExtensions
{
    /// <summary>
    /// Generates debug information for a template context
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="renderTime">The render time</param>
    /// <returns>The debug information represented as a string</returns>
    public static string GenerateDebugInfo(this TemplateContext context, TimeSpan renderTime)
    {
        Validate.IsNotNull(context);

        string Heading(string text)
        {
            var underline = new string('-', text.Length);

            return $"{text}\r\n{underline}\r\n";
        }

        string Detail(string label, object? value)
        {
            return $"\r\n{label}: {value}";
        }

        var builder = new StringBuilder();
        var renderTimeFormatted = $"{renderTime.Milliseconds} milliseconds";

        builder.Append(Heading("Debug Information"));
        builder.Append(Detail("Render Time", renderTimeFormatted));
        builder.Append(Detail("Current Culture Name", CultureInfo.CurrentCulture.Name));
        builder.Append(Detail("Current Culture Description", CultureInfo.CurrentCulture.DisplayName));
        builder.Append(Detail("Default Time Zone ID", NettleEngine.DefaultTimeZone.Id));
        builder.Append(Detail("Default Time Zone Name", NettleEngine.DefaultTimeZone.DisplayName));

        // Generate the property debug info
        builder.Append("\r\n\r\n");
        builder.Append(Heading("Properties"));

        foreach (var property in context.PropertyValues)
        {
            builder.Append(Detail(property.Key, property.Value));
        }

        // Generate the variable debug info
        builder.Append("\r\n\r\n");
        builder.Append(Heading("Variables"));

        foreach (var variable in context.Variables)
        {
            builder.Append(Detail(variable.Key, variable.Value));
        }

        return builder.ToString();
    }
}
