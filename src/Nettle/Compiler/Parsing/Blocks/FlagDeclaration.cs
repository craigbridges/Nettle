namespace Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a flag declaration code block
/// </summary>
/// <param name="Signature">The blocks signature</param>
/// <param name="FlagName">The flag name</param>
internal record class FlagDeclaration(string Signature, string FlagName) : CodeBlock(Signature);
