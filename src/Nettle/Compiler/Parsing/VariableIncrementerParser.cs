﻿namespace Nettle.Compiler.Parsing;

using Nettle.Compiler.Parsing.Blocks;

/// <summary>
/// Represents a variable incrementer block parser
/// </summary>
internal sealed class VariableIncrementerParser : VariableAdjusterParser
{
    /// <summary>
    /// Gets the incrementer operator signature
    /// </summary>
    protected override string AdjusterSignature => @"++";

    /// <summary>
    /// Overrides the base parse logic to return an incrementer code block
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <param name="positionOffSet">The position offset index</param>
    /// <param name="signature">The block signature</param>
    /// <returns>The parsed code block</returns>
    public override CodeBlock Parse(ref string templateContent, ref int positionOffSet, string signature)
    {
        var adjuster = (VariableAdjuster)base.Parse(ref templateContent, ref positionOffSet, signature);

        return new VariableIncrementer(adjuster.Signature, adjuster.VariableName)
        {
            StartPosition = adjuster.StartPosition,
            EndPosition = adjuster.EndPosition
        };
    }
}
