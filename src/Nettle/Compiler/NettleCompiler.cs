namespace Nettle.Compiler
{
    using Nettle.Compiler.Parsing;
    using Nettle.Compiler.Rendering;
    using Nettle.Compiler.Validation;
    using Nettle.Functions;
    using System;

    /// <summary>
    /// Represents the default implantation of a NEttle compiler
    /// </summary>
    public sealed class NettleCompiler : INettleCompiler
    {
        private ITemplateParser _parser;
        private ITemplateRenderer _renderer;
        private ITemplateValidator _validator;
        private IFunctionRepository _functionRepository;
        private IRegisteredTemplateRepository _templateRepository;

        /// <summary>
        /// Constructs the compiler with required dependencies
        /// </summary>
        /// <param name="parser">The template parser</param>
        /// <param name="renderer">The template renderer</param>
        /// <param name="validator">The template validator</param>
        /// <param name="functionRepository">The function repository</param>
        /// <param name="templateRepository">The template repository</param>
        internal NettleCompiler
            (
                ITemplateParser parser,
                ITemplateRenderer renderer,
                ITemplateValidator validator,
                IFunctionRepository functionRepository,
                IRegisteredTemplateRepository templateRepository
            )
        {
            Validate.IsNotNull(parser);
            Validate.IsNotNull(renderer);
            Validate.IsNotNull(validator);
            Validate.IsNotNull(functionRepository);
            Validate.IsNotNull(templateRepository);

            _parser = parser;
            _renderer = renderer;
            _validator = validator;
            _functionRepository = functionRepository;
            _templateRepository = templateRepository;
        }

        /// <summary>
        /// Compiles the template content
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <returns>A function that will generate rendered content</returns>
        public Func<object, string> Compile
            (
                string templateContent
            )
        {
            var parsedTemplate = ParseTemplate
            (
                templateContent
            );

            return Compile(parsedTemplate);
        }
        
        /// <summary>
        /// Compiles a parsed template
        /// </summary>
        /// <param name="parsedTemplate">The parsed template</param>
        /// <returns>A function that will generate rendered content</returns>
        private Func<object, string> Compile
            (
                Template parsedTemplate
            )
        {
            Func<object, string> template =
            (
                (model) => _renderer.Render
                (
                    parsedTemplate,
                    model
                )
            );

            return template;
        }

        /// <summary>
        /// Parses and validates the template content
        /// </summary>
        /// <param name="templateContent">The template content</param>
        /// <returns>The parsed template</returns>
        private Template ParseTemplate
            (
                string templateContent
            )
        {
            var parsedTemplate = _parser.Parse
            (
                templateContent
            );

            var validationResults = _validator.ValidateTemplate
            (
                parsedTemplate
            );

            if (false == validationResults.IsValid)
            {
                throw new NettleValidationException
                (
                    validationResults.Errors
                );
            }
            
            return parsedTemplate;
        }

        /// <summary>
        /// Compiles the view specified as a template
        /// </summary>
        /// <param name="templatePath">The templates file path</param>
        /// <returns>A function that will generate rendered content</returns>
        public Func<object, string> CompileView
            (
                string templatePath
            )
        {
            Validate.IsNotEmpty(templatePath);

            var view = ViewReader.Read
            (
                templatePath
            );

            return Compile(view.Content);
        }

        /// <summary>
        /// Automatically registers all views found in a directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        public void AutoRegisterViews
            (
                string directoryPath
            )
        {
            Validate.IsNotEmpty(directoryPath);

            var matchingViews = ViewReader.ReadAll
            (
                directoryPath
            );

            foreach (var view in matchingViews)
            {
                RegisterTemplate
                (
                    view.Name,
                    view.Content
                );
            }
        }

        /// <summary>
        /// Registers a template to be used with the compiler
        /// </summary>
        /// <param name="name">The template name</param>
        /// <param name="templateContent">The template content</param>
        public void RegisterTemplate
            (
                string name,
                string templateContent
            )
        {
            Validate.IsNotEmpty(name);

            var parsedTemplate = ParseTemplate
            (
                templateContent
            );

            var compiledTemplate = Compile
            (
                parsedTemplate
            );

            var registeredTemplate = new RegisteredTemplate
            (
                name,
                parsedTemplate,
                compiledTemplate
            );

            _templateRepository.Add
            (
                registeredTemplate
            );
        }

        /// <summary>
        /// Registers the function specified with the compiler
        /// </summary>
        /// <param name="function">The function register</param>
        public void RegisterFunction
            (
                IFunction function
            )
        {
            Validate.IsNotNull(function);

            _functionRepository.AddFunction
            (
                function
            );
        }

        /// <summary>
        /// Disables the function specified
        /// </summary>
        /// <param name="functionName">The function name</param>
        public void DisableFunction
            (
                string functionName
            )
        {
            Validate.IsNotEmpty(functionName);

            var function = _functionRepository.GetFunction
            (
                functionName
            );

            function.Disable();
        }

        /// <summary>
        /// Disables all registered functions
        /// </summary>
        public void DisableAllFunctions()
        {
            var functions = _functionRepository.GetAllFunctions();

            foreach (var function in functions)
            {
                if (false == function.Disabled)
                {
                    function.Disable();
                }
            }
        }

        /// <summary>
        /// Enables the function specified
        /// </summary>
        /// <param name="functionName">The function name</param>
        public void EnableFunction
            (
                string functionName
            )
        {
            Validate.IsNotEmpty(functionName);

            var function = _functionRepository.GetFunction
            (
                functionName
            );

            function.Enable();
        }
    }
}
