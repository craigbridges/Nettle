namespace Nettle.Compiler;

using System.Threading.Tasks;

/// <summary>
/// Represents a registered Nettle template
/// </summary>
/// <param name="Name">The template name</param>
/// <param name="ParsedTemplate">The parsed template</param>
/// <param name="CompiledTemplate">The compiled template</param>
internal record class RegisteredTemplate(string Name)
{
    /// <summary>
    /// Ensures the name is alpha numeric
    /// </summary>
    public string Name { get; } = Name.IsAlphaNumeric() 
        ? Name
        : throw new ArgumentException($"'{Name}' is an invalid name for a template. Names must be alphanumeric.");

    /// <summary>
    /// The parsed template code
    /// </summary>
    public required Template ParsedTemplate { get; init; }

    /// <summary>
    /// The compiled template
    /// </summary>
    public required Func<object, CancellationToken, Task<string>> CompiledTemplate { get; init; }
}
