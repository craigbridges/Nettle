namespace Nettle.Compiler.Rendering;

using Nettle.Compiler.Parsing;
using Nettle.Compiler.Parsing.Blocks;
using System.Dynamic;
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
    /// <returns>The resolved value</returns>
    protected object? ResolveValue(ref TemplateContext context, object? rawValue, NettleValueType type)
    {
        if (rawValue == null)
        {
            return null;
        }

        var resolvedValue = default(object);

        switch (type)
        {
            case NettleValueType.ModelBinding:
            {
                resolvedValue = ResolveBindingValue(ref context, rawValue.ToString()!);
                break;
            }
            case NettleValueType.Function:
            {
                if (rawValue != null && rawValue is FunctionCall call)
                {
                    var result = ExecuteFunction(ref context, call);

                    resolvedValue = result.Output;
                    break;
                }
                else
                {
                    throw new NettleRenderException("The function call is invalid.");
                }
            }
            case NettleValueType.Variable:
            {
                resolvedValue = context.ResolveVariableValue(rawValue.ToString()!);
                break;
            }
            case NettleValueType.KeyValuePair:
            {
                var unresolvedPair = (UnresolvedKeyValuePair)rawValue;
                var key = ResolveValue(ref context, unresolvedPair.ParsedKey, unresolvedPair.KeyType);
                var value = ResolveValue(ref context, unresolvedPair.ParsedValue, unresolvedPair.ValueType);

                resolvedValue = new KeyValuePair<object?, object?>(key, value);
                break;
            }
            case NettleValueType.AnonymousType:
            {
                var unresolvedType = (UnresolvedAnonymousType)rawValue;
                var resolvedProperties = new Dictionary<string, object?>();

                foreach (var unresolvedProperty in unresolvedType.Properties)
                {
                    var propertyValue = ResolveValue(ref context, unresolvedProperty.RawValue, unresolvedProperty.ValueType);
                    
                    resolvedProperties.Add(unresolvedProperty.Name, propertyValue);
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
                
                resolvedValue = eo;
                break;
            }
            default:
            {
                resolvedValue = rawValue;
                break;
            }   
        }

        return resolvedValue;
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
    protected virtual object? ResolveBindingValue(ref TemplateContext context, string bindingPath)
    {
        Validate.IsNotEmpty(bindingPath);

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
    /// Executes a function call using the template context specified
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="call">The function call code block</param>
    /// <returns>The function execution result</returns>
    protected FunctionExecutionResult ExecuteFunction(ref TemplateContext context, FunctionCall call)
    {
        Validate.IsNotNull(call);

        var function = FunctionRepository.GetFunction(call.FunctionName);
        var parameterValues = ResolveParameterValues(ref context, call);
        var result = function.Execute(context, parameterValues);

        return result;
    }

    /// <summary>
    /// Resolves the parameter values for a function call
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="call">The function call</param>
    /// <returns>An array of resolved parameter values</returns>
    protected object?[] ResolveParameterValues(ref TemplateContext context, FunctionCall call)
    {
        Validate.IsNotNull(call);

        var parameterValues = new List<object?>();

        foreach (var parameter in call.ParameterValues)
        {
            var value = ResolveValue(ref context, parameter.Value, parameter.Type);

            parameterValues.Add(value);
        }

        return parameterValues.ToArray();
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
