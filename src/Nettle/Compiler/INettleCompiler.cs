namespace Nettle.Compiler
{
    using System;
    using System.IO;

    /// <summary>
    /// Defines a contract for Nettle compiler
    /// </summary>
    public interface INettleCompiler
    {
        /// <summary>
        /// Compiles the template specified
        /// </summary>
        /// <param name="template">The template to compile</param>
        /// <returns>An action that will write to a text writer</returns>
        Action<TextWriter, object> Compile
        (
            TextReader template
        );

        /// <summary>
        /// Compiles the template specified
        /// </summary>
        /// <param name="template">The template to compile</param>
        /// <returns>A function that will generate rendered content</returns>
        Func<object, string> Compile
        (
            string template
        );

        /// <summary>
        /// Compiles the view specified as a template
        /// </summary>
        /// <param name="templatePath">The templates file path</param>
        /// <returns>A function that will generate rendered content</returns>
        Func<object, string> CompileView
        (
            string templatePath
        );
    }
}
