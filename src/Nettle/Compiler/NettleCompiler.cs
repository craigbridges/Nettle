namespace Nettle.Compiler
{
    using Nettle.Compiler.Parsing;
    using Nettle.Functions;
    using System;
    using System.IO;

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
        /// Compiles the template specified
        /// </summary>
        /// <param name="template">The template to compile</param>
        /// <returns>An action that will write to a text writer</returns>
        public Action<TextWriter, object> Compile
            (
                TextReader template
            )
        {
            Validate.IsNotNull(template);

            throw new NotImplementedException();
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

            var fileContents = ReadViewContent
            (
                templatePath
            );

            return Compile(fileContents);
        }

        /// <summary>
        /// Reads the contents of a template view into a string
        /// </summary>
        /// <param name="filePath">The views file path</param>
        /// <returns>The views content as a string</returns>
        private string ReadViewContent
            (
                string filePath
            )
        {
            Validate.IsNotEmpty(filePath);

            // TODO: load file and extract into string

            throw new NotImplementedException();
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

            // TODO: ensure directoryPath points to a valid directory
            // TODO: scan for all Nettle specific views in the directory
            // TODO: use the filename (excluding extension) as the registered name
            // TODO: compile the each view and add to repository

            throw new NotImplementedException();
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

            var template = Compile
            (
                templateContent
            );

            var registeredTemplate = new RegisteredTemplate
            (
                name,
                template
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
    }
}
