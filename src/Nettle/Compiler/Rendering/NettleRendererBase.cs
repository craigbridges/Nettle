namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing;
using Nettle.Compiler.Parsing.Blocks;
using System.Dynamic;
using System.Threading.Tasks;
using System.Xml;

/// <summary>
/// Represents a base class for all Nettle renderer's
/// </summary>
internal abstract class NettleRendererBase
{
    public NettleRendererBase(IFunctionRepository functionRepository)
    {
        Validate.IsNotNull(functionRepository);

        FunctionRepository = functionRepository;
    }

    /// <summary>
    /// Gets the function repository
    /// </summary>
    protected IFunctionRepository FunctionRepository { get; init; }

    /// <summary>
    /// Resolves a value by converting it to the Nettle value type specified
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="rawValue">The raw value</param>
    /// <param name="type">The value type</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The resolved value</returns>
    protected async Task<object?> ResolveValue(TemplateContext context, object? rawValue, NettleValueType type, CancellationToken cancellationToken)
    {
        if (rawValue == null)
        {
            return null;
        }
        else
        {
            return type switch
            {
                NettleValueType.Enum => ResolveEnum(),
                NettleValueType.ModelBinding => ResolveModelBinding(),
                NettleValueType.Function => await ResolveFunction(),
                NettleValueType.Variable => ResolveVariable(),
                NettleValueType.KeyValuePair => await ResolveKeyValuePair(),
                NettleValueType.AnonymousType => await ResolveAnonymousType(),
                _ => rawValue,
            };
        }

        object ResolveEnum()
        {
            if (rawValue.GetType().IsEnum)
            {
                return rawValue;
            }
            else if (rawValue is string enumSignature)
            {
                return EnumParser.Parse(enumSignature);
            }
            else
            {
                throw new NettleRenderException($"The enum signature '{rawValue}' is invalid.");
            }
        }

        object? ResolveModelBinding()
        {
            return ResolveBindingValue(context, rawValue.ToString()!);
        }

        async Task<object?> ResolveFunction()
        {
            if (rawValue != null && rawValue is FunctionCall call)
            {
                var result = await ExecuteFunction(context, call, cancellationToken);

                return result.Output;
            }
            else
            {
                throw new NettleRenderException($"The function call '{rawValue}' is invalid.");
            }
        }

        object? ResolveVariable()
        {
            return context.ResolveVariableValue(rawValue.ToString()!);
        }

        async Task<object?> ResolveKeyValuePair()
        {
            var unresolvedPair = (UnresolvedKeyValuePair)rawValue;

            var keyTask = ResolveValue(context, unresolvedPair.ParsedKey, unresolvedPair.KeyType, cancellationToken);
            var valueTask = ResolveValue(context, unresolvedPair.ParsedValue, unresolvedPair.ValueType, cancellationToken);

            var keyValueResults = await Task.WhenAll(keyTask, valueTask);

            return new KeyValuePair<object?, object?>(keyValueResults[0], keyValueResults[1]);
        }

        async Task<object?> ResolveAnonymousType()
        {
            var unresolvedType = (UnresolvedAnonymousType)rawValue;
            var resolveTasks = new List<Task<KeyValuePair<string, object?>>>();

            foreach (var property in unresolvedType.Properties)
            {
                resolveTasks.Add(ResolveProperty(property));
            }

            var resolvedProperties = new Dictionary<string, object?>(await Task.WhenAll(resolveTasks));

            async Task<KeyValuePair<string, object?>> ResolveProperty(UnresolvedAnonymousTypeProperty property)
            {
                var propertyRawValue = property.RawValue;
                var propertyValueType = property.ValueType;

                var propertyValue = await ResolveValue(context, propertyRawValue, propertyValueType, cancellationToken);

                return new(property.Name, propertyValue);
            }

            // Convert the property dictionary to an expando object
            dynamic eo = resolvedProperties.Aggregate
            (
                new ExpandoObject() as IDictionary<string, object?>,
                (a, p) =>
                {
                    a.Add(p.Key, p.Value);
                    return a;
                }
            );

            return eo;
        }
    }

    /// <summary>
    /// Resolves a model binding value from the context and binding path
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="bindingPath">The bindings path</param>
    /// <returns>The model bindings value</returns>
    /// <remarks>
    /// A check is made to see if the binding path refers to a variable.
    /// If not variable is found then it is assumed to be a property.
    /// </remarks>
    protected virtual object? ResolveBindingValue(TemplateContext context, string bindingPath)
    {
        var pathInfo = new NettlePath(bindingPath);
        var name = pathInfo[0].Name;
        var variableFound = context.Variables.ContainsKey(name);

        // Check if it's a variable or property
        if (variableFound)
        {
            return context.ResolveVariableValue(bindingPath);
        }
        else
        {
            return context.ResolvePropertyValue(bindingPath);
        }
    }
    
    /// <summary>
    /// Asynchronously executes a function call using the template context specified
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="call">The function call code block</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The function execution result</returns>
    protected async Task<FunctionExecutionResult> ExecuteFunction(TemplateContext context, FunctionCall call, CancellationToken cancellationToken)
    {
        var function = FunctionRepository.GetFunction(call.FunctionName);
        var parameterValues = await ResolveParameterValues(context, call, cancellationToken);
        
        var request = new FunctionExecutionRequest()
        {
            Context = context,
            ParameterValues = parameterValues
        };

        var result = await function.Execute(request, cancellationToken);

        return result;
    }

    /// <summary>
    /// Asynchronously resolves the parameter values for a function call
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="call">The function call</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An array of resolved parameter values</returns>
    protected async Task<object?[]> ResolveParameterValues(TemplateContext context, FunctionCall call, CancellationToken cancellationToken)
    {
        var resolveTasks = new List<Task<object?>>();

        foreach (var parameter in call.ParameterValues)
        {
            resolveTasks.Add(ResolveValue(context, parameter.Value, parameter.Type, cancellationToken));
        }

        var parameterValues = await Task.WhenAll(resolveTasks);

        return parameterValues;
    }

    /// <summary>
    /// Converts a generic object to a string representation
    /// </summary>
    /// <param name="value">The value to convert</param>
    /// <param name="flags">The template flags</param>
    /// <returns>A string representation of the value</returns>
    protected virtual string ToString(object? value, params TemplateFlag[] flags)
    {
        if (value == null)
        {
            return String.Empty;
        }
        else
        {
            var valueType = value.GetType();

            if (valueType == typeof(string))
            {
                return (string)value;
            }
            else if (valueType == typeof(DateTime) || valueType == typeof(DateTime?))
            {
                var dateValue = (DateTime)value;
                var forceUtc = flags.Contains(TemplateFlag.UseUtc);

                if (forceUtc && dateValue.Kind == DateTimeKind.Local)
                {
                    return dateValue.ToUniversalTime().ToString();
                }
                else
                {
                    return dateValue.ToString();
                }
            }
            else if (valueType == typeof(XmlDocument))
            {
                return ((XmlDocument)value).Stringify();
            }
            else
            {
                return value.ToString() ?? String.Empty;
            }
        }
    }
}
