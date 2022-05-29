﻿namespace Nettle.Compiler.Parsing;

using Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a variable declaration code block parser
/// </summary>
internal class VariableParser : NettleParser, IBlockParser
{
    /// <summary>
    /// Gets the signature bodies prefix value
    /// </summary>
    protected virtual string Prefix => "var ";

    /// <summary>
    /// Determines if a signature matches the block type of the parser
    /// </summary>
    /// <param name="signatureBody">The signature body</param>
    /// <returns>True, if it matches; otherwise false</returns>
    public virtual bool Matches(string signatureBody) => signatureBody.StartsWith(Prefix);

    /// <summary>
    /// Parses the code block signature into a code block object
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <param name="positionOffSet">The position offset index</param>
    /// <param name="signature">The block signature</param>
    /// <returns>The parsed code block</returns>
    public virtual CodeBlock Parse(ref string templateContent, ref int positionOffSet, string signature)
    {
        var body = UnwrapSignatureBody(signature);
        var nameIndex = this.Prefix.Length;
        var equalsIndex = body.IndexOf('=');

        if (equalsIndex == -1)
        {
            throw new NettleParseException
            (
                $"The variable declaration '{signature}' has invalid syntax.",
                positionOffSet
            );
        }

        // Extract the variable name and value signature
        var variableName = body.Crop(nameIndex, equalsIndex - 1).Trim();
        var valueSignature = body.Crop(equalsIndex + 1).Trim();

        var type = ResolveType(valueSignature);
        var value = type.ParseValue(valueSignature);

        var isValidName = IsValidVariableName(variableName);

        if (false == isValidName)
        {
            throw new NettleParseException
            (
                $"The variable name '{variableName}' is invalid.",
                positionOffSet
            );
        }

        var startPosition = positionOffSet;
        var endPosition = (startPosition + signature.Length);

        TrimTemplate(ref templateContent, ref positionOffSet, signature);

        var declaration = new VariableDeclaration(signature, variableName)
        {
            StartPosition = startPosition,
            EndPosition = endPosition,
            AssignedValueSignature = valueSignature,
            AssignedValue = value,
            ValueType = type
        };

        return declaration;
    }

    /// <summary>
    /// Determines if a variable name is valid
    /// </summary>
    /// <param name="name">The variable name</param>
    /// <returns>True, if the variable name is valid; otherwise false</returns>
    /// <remarks>
    /// The rules for validating a variable name are:
    /// 
    /// - The name must not contain spaces
    /// - The name must start with a letter
    /// - Only letters and numbers are allowed
    /// </remarks>
    public static bool IsValidVariableName(string name)
    {
        if (String.IsNullOrEmpty(name) || name.Contains(' '))
        {
            return false;
        }
        else
        {
            var firstChar = name.First();

            if (false == Char.IsLetter(firstChar))
            {
                return false;
            }
            else if (false == name.IsAlphaNumeric())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
