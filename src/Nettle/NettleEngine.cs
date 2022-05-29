namespace Nettle
{
    /// <summary>
    /// Represents the entry point for all Nettle actions
    /// </summary>
    public static class NettleEngine
    {
        private static INettleCompiler? _compiler;
        private static readonly object _engineLock = new();
        private static TimeZoneInfo _defaultTimeZone = TimeZoneInfo.Local;

        private static readonly List<INettleResolver> _resolvers = new()
        {
            new DefaultNettleResolver()
        };

        /// <summary>
        /// Defines the file extension for Nettle views
        /// </summary>
        internal const string ViewFileExtension = "nettle";

        /// <summary>
        /// Gets the default time zone to use for dates
        /// </summary>
        public static TimeZoneInfo DefaultTimeZone => _defaultTimeZone;

        /// <summary>
        /// Sets the default time zone to use for all date times
        /// </summary>
        /// <param name="timeZoneId">The time zone ID</param>
        public static void SetDefaultTimeZone(string timeZoneId)
        {
            Validate.IsNotEmpty(timeZoneId);
            
            lock (_engineLock)
            {
                _defaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
        }

        /// <summary>
        /// Registers resolvers to be used when generating compilers
        /// </summary>
        /// <param name="resolvers">The resolvers to register</param>
        public static void RegisterResolvers(params INettleResolver[] resolvers)
        {
            Validate.IsNotNull(resolvers);

            lock (_engineLock)
            {
                foreach (var resolver in resolvers)
                {
                    var registered = _resolvers.Any(x => x.GetType() == resolver.GetType());

                    if (false == registered)
                    {
                        _resolvers.Add(resolver);
                    }
                }

                _compiler = null;
            }
        }
        
        /// <summary>
        /// Gets a Nettle compiler instance
        /// </summary>
        /// <returns>The compiler</returns>
        public static INettleCompiler GetCompiler()
        {
            if (_compiler == null)
            {
                lock (_engineLock)
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
        public static INettleCompiler GetCompiler(params IFunction[] customFunctions)
        {
            return GenerateCompiler(customFunctions);
        }

        /// <summary>
        /// Generates a new Nettle compiler with custom functions
        /// </summary>
        /// <param name="customFunctions">The custom functions</param>
        /// <returns>The compiler generated</returns>
        private static INettleCompiler GenerateCompiler(params IFunction[] customFunctions)
        {
            var templateRepository = new RegisteredTemplateRepository();
            var functionRepository = new FunctionRepository(_resolvers.ToArray());

            var blockifier = new Blockifier();
            var parser = new TemplateParser(blockifier);
            var renderer = new TemplateRenderer(functionRepository, templateRepository);
            var validator = new TemplateValidator(functionRepository);

            var compiler = new NettleCompiler(parser, renderer, validator, functionRepository, templateRepository);

            if (customFunctions != null)
            {
                foreach (var function in customFunctions)
                {
                    compiler.RegisterFunction(function);
                }
            }

            return compiler;
        }
    }
}
