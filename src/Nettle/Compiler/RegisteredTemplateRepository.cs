namespace Nettle.Compiler;

internal sealed class RegisteredTemplateRepository : IRegisteredTemplateRepository
{
    private readonly Dictionary<string, RegisteredTemplate> _templates;

    public RegisteredTemplateRepository()
    {
        _templates = new Dictionary<string, RegisteredTemplate>();
    }

    public void Add(RegisteredTemplate template)
    {
        Validate.IsNotNull(template);

        var name = template.Name;
        var found = _templates.ContainsKey(name);

        if (found)
        {
            throw new InvalidOperationException
            (
                $"A template with the name '{name}' has already been registered."
            );
        }

        _templates.Add(name, template);
    }

    public RegisteredTemplate Get(string name)
    {
        Validate.IsNotEmpty(name);

        if (false == _templates.ContainsKey(name))
        {
            throw new KeyNotFoundException
            (
                $"No template has been registered with the name '{name}'."
            );
        }

        return _templates[name];
    }

    public IEnumerable<RegisteredTemplate> GetAll()
    {
        return _templates.Select(item => item.Value);
    }

    public void Remove(string name)
    {
        Validate.IsNotEmpty(name);

        if (false == _templates.ContainsKey(name))
        {
            throw new KeyNotFoundException
            (
                $"No template has been registered with the name '{name}'."
            );
        }

        _templates.Remove(name);
    }
}
