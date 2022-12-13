namespace Nettle.Functions;

using Nettle.Compiler;

/// <summary>
/// Represents a base class for a Nettle function
/// </summary>
public abstract class FunctionBase : IFunction
{
    protected FunctionBase()
    {
        Parameters = new List<FunctionParameter>();
    }

    /// <summary>
    /// Gets the name of the function (this is the value used to call the function)
    /// </summary>
    public virtual string Name => GetType().Name.Replace("Function", string.Empty);

    /// <summary>
    /// Gets a description for the function (for documentation purposes)
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Gets a flag indicating if the function is disabled
    /// </summary>
    public bool Disabled { get; private set; }

    /// <summary>
    /// Enables the function
    /// </summary>
    public void Enable()
    {
        if (false == Disabled)
        {
            throw new InvalidOperationException
            (
                $"The function {Name} has already been enabled."
            );
        }

        Disabled = false;
    }

    /// <summary>
    /// Disables the function
    /// </summary>
    public void Disable()
    {
        if (Disabled)
        {
            throw new InvalidOperationException
            (
                $"The function {Name} has already been disabled."
            );
        }

        Disabled = true;
    }

    /// <summary>
    /// Gets a list of function parameters
    /// </summary>
    protected List<FunctionParameter> Parameters { get; private set; }

    /// <summary>
    /// Defines a required parameter using the details specified
    /// </summary>
    /// <param name="name">The name</param>
    /// <param name="description">The description</param>
    /// <param name="dataType">The data type</param>
    /// <param name="defaultValue">The default value (optional)</param>
    protected virtual void DefineRequiredParameter(string name, string description, Type dataType, object? defaultValue = null)
    {
        var optionalParameters = GetOptionalParameters();

        // Ensure no optional parameters have been defined
        if (optionalParameters.Any())
        {
            throw new InvalidOperationException
            (
                "Required parameters must be defined before optional parameters."
            );
        }

        var configuration = new FunctionParameterConfiguration(name, dataType)
        {
            Description = description,
            DefaultValue = defaultValue,
            Optional = false
        };

        DefineParameter(configuration);
    }

    /// <summary>
    /// Defines an optional parameter using the details specified
    /// </summary>
    /// <param name="name">The name</param>
    /// <param name="description">The description</param>
    /// <param name="dataType">The data type</param>
    /// <param name="defaultValue">The default value (optional)</param>
    protected virtual void DefineOptionalParameter(string name, string description, Type dataType, object? defaultValue = null)
    {
        var configuration = new FunctionParameterConfiguration(name, dataType)
        {
            Description = description,
            DefaultValue = defaultValue,
            Optional = true
        };

        DefineParameter(configuration);
    }

    /// <summary>
    /// Defines a parameter for the function
    /// </summary>
    /// <param name="configuration">The parameter configuration</param>
    protected virtual void DefineParameter(FunctionParameterConfiguration configuration)
    {
        if (Parameters == null)
        {
            Parameters = new List<FunctionParameter>();
        }

        var matchFound = Parameters.Any(x => x.Name.Equals(configuration.Name, StringComparison.OrdinalIgnoreCase));

        if (matchFound)
        {
            throw new InvalidOperationException
            (
                $"The parameter '{configuration.Name}' has already been defined."
            );
        }

        Parameters.Add(new FunctionParameter(this, configuration));
    }

    /// <summary>
    /// Gets a collection of parameters for the function
    /// </summary>
    public virtual IEnumerable<FunctionParameter> GetAllParameters() => Parameters;

    /// <summary>
    /// Gets a collection of parameters that are required
    /// </summary>
    /// <returns>A collection of matching function parameters</returns>
    public virtual IEnumerable<FunctionParameter> GetRequiredParameters() => Parameters.Where(x => x.IsRequired());

    /// <summary>
    /// Gets a collection of parameters that are optional
    /// </summary>
    /// <returns>A collection of matching function parameters</returns>
    public virtual IEnumerable<FunctionParameter> GetOptionalParameters() => Parameters.Where(x => x.Optional);

    /// <summary>
    /// Gets the function parameter by the name specified
    /// </summary>
    /// <param name="name">The name of the parameter to get</param>
    /// <returns>The matching parameter</returns>
    public virtual FunctionParameter GetParameter(string name)
    {
        Validate.IsNotEmpty(name);

        var parameter = Parameters.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (parameter == null)
        {
            throw new KeyNotFoundException
            (
                $"No parameter was found matching the name '{name}'."
            );
        }

        return parameter;
    }

    /// <summary>
    /// Gets a specific parameter value
    /// </summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="parameterValues">An array of values</param>
    /// <returns>The parameter value found</returns>
    protected virtual object? GetParameterValue(string parameterName, params object?[] parameterValues)
    {
        var parameter = GetParameter(parameterName);
        var index = Parameters.IndexOf(parameter);

        if (index >= parameterValues.Length)
        {
            return parameter.DefaultValue;
        }
        else
        {
            return parameterValues[index];
        }
    }

    /// <summary>
    /// Gets a specific parameter value as the type specified
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="parameterValues">An array of values</param>
    /// <returns>The parameter value found</returns>
    protected virtual T? GetParameterValue<T>(string parameterName, params object?[] parameterValues)
    {
        var rawValue = GetParameterValue(parameterName, parameterValues);

        return new GenericObjectToTypeConverter<T>().Convert(rawValue);
    }

    /// <summary>
    /// Converts all the parameter values specified to an array of doubles
    /// </summary>
    /// <param name="parameterValues">An array of values</param>
    /// <returns>An array of doubles</returns>
    protected virtual double[] ConvertToNumbers(params object?[] parameterValues)
    {
        Validate.IsNotNull(parameterValues);

        var numbers = new List<double>();

        foreach (var value in parameterValues)
        {
            if (value != null)
            {
                if (value.ToString()!.IsNumeric())
                {
                    numbers.Add(Double.Parse(value.ToString()!));
                }
                else if (value.GetType().IsEnumerable())
                {
                    var items = new List<object>();

                    foreach (var item in (IEnumerable)value)
                    {
                        items.Add(item);
                    }

                    var nestedNumbers = ConvertToNumbers(items.ToArray());

                    numbers.AddRange(nestedNumbers);
                }
                else
                {
                    throw new ArgumentException("Only numeric values are supported.");
                }
            }
        }

        return numbers.ToArray();
    }

    /// <summary>
    /// Extracts an array of key value pairs from the parameter values
    /// </summary>
    /// <param name="parameterValues">The parameter values</param>
    /// <param name="startIndex">The start index (optional)</param>
    /// <returns></returns>
    protected virtual Dictionary<TKey, TValue?> ExtractKeyValuePairs<TKey, TValue>(object?[] parameterValues, int startIndex = 0)
        where TKey : notnull
    {
        var keyValuePairs = new Dictionary<TKey, TValue?>();

        if (parameterValues.Length >= (startIndex + 1))
        {
            for (var i = startIndex; i < parameterValues.Length; i++)
            {
                var nextValue = parameterValues[i];
                var nextType = nextValue?.GetType();

                if (nextValue == null)
                {
                    throw new ArgumentException("The parameter values cannot be null.");
                }

                if (nextType != typeof(KeyValuePair<object?, object?>))
                {
                    throw new ArgumentException("The parameter values must be key value pair.");
                }

                var pair = (KeyValuePair<object?, object?>)nextValue;

                keyValuePairs.Add((TKey)pair.Key!, (TValue?)pair.Value);
            }
        }

        return keyValuePairs;
    }

    /// <summary>
    /// Executes the function against a template context and parameter values
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="parameterValues">The parameter values</param>
    /// <returns>The execution result</returns>
    public virtual FunctionExecutionResult Execute(TemplateContext context, params object?[] parameterValues)
    {
        Validate.IsNotNull(context);

        if (Disabled)
        {
            throw new InvalidOperationException($"The Nettle function '{Name}' has been disabled.");
        }

        var expectedCount = GetRequiredParameters().Count();
        var parameterCount = 0;

        if (parameterValues != null)
        {
            parameterCount = parameterValues.Length;
        }

        if (expectedCount > 0)
        {
            if (parameterValues == null || parameterCount < expectedCount)
            {
                throw new ArgumentException
                (
                    $"{expectedCount} parameters were expected, {parameterCount} were supplied."
                );
            }
        }

        if (parameterValues != null && Parameters.Any())
        {
            var counter = 0;

            foreach (var value in parameterValues)
            {
                if (counter < Parameters.Count)
                {
                    var parameter = Parameters[counter];

                    if (false == parameter.IsValidParameterValue(value))
                    {
                        throw new ArgumentException
                        (
                            $"'{value}' is not valid for the parameter {parameter.Name}."
                        );
                    }
                }

                counter++;
            }
        }

        parameterValues ??= Array.Empty<object?>();

        var output = GenerateOutput(context, parameterValues);

        return new FunctionExecutionResult(this, output, parameterValues);
    }

    /// <summary>
    /// When implemented in derived class, generates the functions output value
    /// </summary>
    /// <param name="context">The template context</param>
    /// <param name="parameterValues">The parameter values</param>
    /// <returns>The output value</returns>
    protected abstract object? GenerateOutput(TemplateContext context, params object?[] parameterValues);

    /// <summary>
    /// Provides a syntax description of the content function
    /// </summary>
    /// <returns>A string representing the syntax of the function</returns>
    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append('@');
        builder.Append(Name);
        builder.Append('(');

        var parameterCount = 0;

        foreach (var parameter in Parameters)
        {
            if (parameterCount > 0)
            {
                builder.Append(", ");
            }

            if (parameter.DataType == typeof(string))
            {
                builder.Append('"');
                builder.Append(parameter.Name);
                builder.Append('"');
            }
            else
            {
                builder.Append(parameter.Name);
            }

            parameterCount++;
        }

        builder.Append(')');

        return builder.ToString();
    }
}
