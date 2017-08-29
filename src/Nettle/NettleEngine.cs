namespace Nettle
{
    using Nettle.Compiler;
    using Nettle.Compiler.Parsing;
    using Nettle.Compiler.Rendering;
    using Nettle.Functions;

    /// <summary>
    /// Represents the entry point for all Nettle actions
    /// </summary>
    public static class NettleEngine
    {
        private static INettleCompiler _compiler;
        private static object _compilerLock = new object();

        /// <summary>
        /// Defines the file extension for Nettle views
        /// </summary>
        internal const string ViewFileExtension = "nettle";

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
        /// Gets a Nettle compiler with custom functions
        /// </summary>
        /// <param name="customFunctions">The custom functions</param>
        /// <returns>The compiler</returns>
        public static INettleCompiler GetCompiler
            (
                params IFunction[] customFunctions
            )
        {
            return GenerateCompiler
            (
                customFunctions
            );
        }

        /// <summary>
        /// Generates a new Nettle compiler with custom functions
        /// </summary>
        /// <param name="customFunctions">The custom functions</param>
        /// <returns>The compiler generated</returns>
        private static INettleCompiler GenerateCompiler
            (
                params IFunction[] customFunctions
            )
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
                functionRepository,
                templateRepository
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

            if (customFunctions != null)
            {
                foreach (var function in customFunctions)
                {
                    compiler.RegisterFunction
                    (
                        function
                    );
                }
            }

            return compiler;
        }
    }
}
