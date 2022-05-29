namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a model binding code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="BindingPath">The bindings path</param>
/// <remarks>
/// The binding path can contain nested properties and 
/// these are donated by using a dot "." separator.
/// </remarks>
internal record class ModelBinding(string Signature, string BindingPath) : CodeBlock(Signature);
