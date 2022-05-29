namespace Nettle.Compiler;

/// <summary>
/// Represents a registered Nettle template
/// </summary>
/// <param name="Name">The template name</param>
/// <param name="ParsedTemplate">The parsed template</param>
/// <param name="CompiledTemplate">The compiled template</param>
internal record class RegisteredTemplate(string Name, Template ParsedTemplate, Func<object, string> CompiledTemplate)
{
    public string Name { get; } = Name.IsAlphaNumeric() 
        ? Name
        : throw new ArgumentException($"'{Name}' is an invalid name for a template. Names must be alphanumeric.");
}
