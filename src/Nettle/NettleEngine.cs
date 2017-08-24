namespace Nettle
{
    using Nettle.Compiler;
    using Nettle.Compiler.Parsing;
    using Nettle.Functions;
    using System;

    /// <summary>
    /// Represents the entry point for all Nettle actions
    /// </summary>
    public static class NettleEngine
    {
        private static INettleCompiler _compiler;
        private static object _compilerLock = new object();

        /// <summary>
        /// Gets a Nettle compiler instance
        /// </summary>
        /// <returns>The compiler</returns>
        public static INettleCompiler GetCompiler()
        {
            if (_compiler == null)
            {
                lock (_compilerLock)
                {
                    _compiler = GenerateCompiler();
                }
            }

            return _compiler;
        }

        /// <summary>
        /// Gets a Nettle compiler using the activator specified
        /// </summary>
        /// <param name="activator">The activator</param>
        /// <returns>The compiler</returns>
        public static INettleCompiler GetCompiler
            (
                INettleActivator activator
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a new Nettle compiler
        /// </summary>
        /// <returns>The compiler generated</returns>
        private static INettleCompiler GenerateCompiler()
        {
            var blockifier = new Blockifier();
            var functionRepository = new FunctionRepository();
            var templateRepository = new RegisteredTemplateRepository();

            var parser = new TemplateParser
            (
                blockifier
            );

            var renderer = new TemplateRenderer
            (
                functionRepository
            );

            var validator = new TemplateValidator
            (
                functionRepository
            );

            var compiler = new NettleCompiler
            (
                parser,
                renderer,
                validator,
                functionRepository,
                templateRepository
            );

            return compiler;
        }
    }
}
