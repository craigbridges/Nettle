namespace Nettle.Compiler;

using System.Threading.Tasks;

public sealed class NettleCompiler : INettleCompiler
{
    private readonly ITemplateParser _parser;
    private readonly ITemplateRenderer _renderer;
    private readonly ITemplateValidator _validator;
    private readonly IFunctionRepository _functionRepository;
    private readonly IRegisteredTemplateRepository _templateRepository;

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

    public Func<object, CancellationToken, Task<string>> Compile(string templateContent)
    {
        var parsedTemplate = ParseTemplate(templateContent);

        return Compile(parsedTemplate);
    }
    
    private Func<object, CancellationToken, Task<string>> Compile(Template parsedTemplate)
    {
        Task<string> template(object model, CancellationToken cancellationToken)
        {
            return _renderer.Render(parsedTemplate, model, cancellationToken);
        }

        return template;
    }

    private Template ParseTemplate(string templateContent)
    {
        var parsedTemplate = _parser.Parse(templateContent);
        var validationResults = _validator.ValidateTemplate(parsedTemplate);

        if (false == validationResults.IsValid)
        {
            throw new NettleValidationException(validationResults.Errors);
        }
        
        return parsedTemplate;
    }

    public async Task<Func<object, CancellationToken, Task<string>>> CompileView(string templatePath, CancellationToken cancellationToken)
    {
        var view = await ViewReader.ReadAsync(templatePath, cancellationToken);

        return Compile(view.Content);
    }

    public async Task AutoRegisterViews(string directoryPath, CancellationToken cancellationToken)
    {
        var matchingViews = await ViewReader.ReadAllAsync(directoryPath, cancellationToken).ConfigureAwait(false);

        foreach (var view in matchingViews)
        {
            RegisterTemplate(view.Name, view.Content);
        }
    }

    public void RegisterTemplate(string name, string templateContent)
    {
        var parsedTemplate = ParseTemplate(templateContent);
        var compiledTemplate = Compile(parsedTemplate);

        var registeredTemplate = new RegisteredTemplate(name)
        {
            ParsedTemplate = parsedTemplate,
            CompiledTemplate = compiledTemplate
        };

        _templateRepository.Add(registeredTemplate);
    }

    public void RegisterFunction(IFunction function)
    {
        _functionRepository.AddFunction(function);
    }

    public void DisableFunction(string functionName)
    {
        var function = _functionRepository.GetFunction(functionName);

        function.Disable();
    }

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

    public void EnableFunction(string functionName)
    {
        var function = _functionRepository.GetFunction(functionName);

        function.Enable();
    }
}
