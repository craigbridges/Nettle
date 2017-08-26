﻿namespace Nettle.Compiler
{
    using Nettle.Functions;
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
        /// Compiles the template content specified
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <returns>A function that will generate rendered content</returns>
        Func<object, string> Compile
        (
            string templateContent
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

        /// <summary>
        /// Automatically registers all views found in a directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        void AutoRegisterViews
        (
            string directoryPath
        );

        /// <summary>
        /// Registers a template to be used with the compiler
        /// </summary>
        /// <param name="name">The template name</param>
        /// <param name="templateContent">The template content</param>
        void RegisterTemplate
        (
            string name,
            string templateContent
        );

        /// <summary>
        /// Registers the function specified with the compiler
        /// </summary>
        /// <param name="function">The function register</param>
        void RegisterFunction
        (
            IFunction function
        );
    }
}
