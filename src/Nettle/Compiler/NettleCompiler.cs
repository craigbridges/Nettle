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

    public Func<object, Task<string>> Compile(string templateContent)
    {
        var parsedTemplate = ParseTemplate(templateContent);

        return Compile(parsedTemplate);
    }
    
    private Func<object, string> Compile(Template parsedTemplate)
    {
        Task<string> template(object model) => _renderer.Render(parsedTemplate, model);

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

    public Func<object, string> CompileView(string templatePath)
    {
        Validate.IsNotEmpty(templatePath);

        var view = ViewReader.Read(templatePath);

        return Compile(view.Content);
    }

    public async Task AutoRegisterViewsAsync(string directoryPath, CancellationToken cancellationToken)
    {
        Validate.IsNotEmpty(directoryPath);

        var matchingViews = await ViewReader.ReadAllAsync(directoryPath, cancellationToken).ConfigureAwait(false);

        foreach (var view in matchingViews)
        {
            RegisterTemplate(view.Name, view.Content);
        }
    }

    public void RegisterTemplate(string name, string templateContent)
    {
        Validate.IsNotEmpty(name);

        var parsedTemplate = ParseTemplate(templateContent);
        var compiledTemplate = Compile(parsedTemplate);

        var registeredTemplate = new RegisteredTemplate(name, parsedTemplate, compiledTemplate);

        _templateRepository.Add(registeredTemplate);
    }

    public void RegisterFunction(IFunction function)
    {
        Validate.IsNotNull(function);

        _functionRepository.AddFunction(function);
    }

    public void DisableFunction(string functionName)
    {
        Validate.IsNotEmpty(functionName);

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
        Validate.IsNotEmpty(functionName);

        var function = _functionRepository.GetFunction(functionName);

        function.Enable();
    }
}
