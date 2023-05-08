namespace Nettle.Compiler;

using Nettle.Compiler.Parsing;
using System.Dynamic;

/// <summary>
/// Represents a Nettle template context
/// </summary>
public sealed class TemplateContext
{
    internal TemplateContext(object model, params TemplateFlag[] flags)
    {
        Model = model;
        PartialCallStack = new List<string>();
        PropertyValues = new Dictionary<string, object?>();
        Variables = new Dictionary<string, object?>();

        if (flags == null)
        {
            Flags = Array.Empty<TemplateFlag>();
        }
        else
        {
            Flags = flags;
        }

        PopulatePropertyValues(model);
    }

    private TemplateContext(TemplateContext parent, object model, params TemplateFlag[] flags)
        : this(model, flags)
    {
        Parent = parent;
    }

    /// <summary>
    /// Gets the parent template context
    /// </summary>
    private TemplateContext? Parent { get; set; }

    /// <summary>
    /// Determines if this is the root template context
    /// </summary>
    /// <returns>True, if it's the root context; otherwise false</returns>
    public bool IsRoot() => Parent == null;

    /// <summary>
    /// Gets the templates model
    /// </summary>
    public object Model { get; private set; }

    /// <summary>
    /// Gets an array of template flags
    /// </summary>
    public TemplateFlag[] Flags { get; private set; }

    /// <summary>
    /// Determines if a specific template flag has been set
    /// </summary>
    /// <param name="flag">The flag</param>
    /// <returns>True, if it has been set; otherwise false</returns>
    public bool IsFlagSet(TemplateFlag flag) => Flags.Contains(flag);

    /// <summary>
    /// Gets a call stack of partials that have been rendered
    /// </summary>
    internal List<string> PartialCallStack { get; private set; }

    /// <summary>
    /// Gets the contexts property values
    /// </summary>
    public Dictionary<string, object?> PropertyValues { get; private set; }

    /// <summary>
    /// Populates the property values by scanning the model
    /// </summary>
    /// <param name="model">The model data</param>
    private void PopulatePropertyValues(object model)
    {
        if (model != null)
        {
            if (model.GetType() == typeof(ExpandoObject))
            {
                var items = (IDictionary<string, object?>)model;

                foreach (var item in items)
                {
                    PropertyValues.Add(item.Key, item.Value);
                }
            }
            else
            {
                var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    var indexParameters = property.GetIndexParameters();

                    if (indexParameters == null || indexParameters.Length == 0)
                    {
                        var propertyValue = property.GetValue(model);

                        PropertyValues.Add(property.Name, propertyValue);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Resolves a property value from the property path specified
    /// </summary>
    /// <param name="propertyPath">The property path</param>
    /// <returns>The property value found</returns>
    public object? ResolvePropertyValue(string propertyPath)
    {
        Validate.IsNotEmpty(propertyPath);

        var isModelRef = IsModelReference(propertyPath);

        if (isModelRef)
        {
            return Model;
        }

        var pathInfo = new NettlePath(propertyPath);

        // Try to resolve a nested property if it looks like one
        if (pathInfo.IsNested)
        {
            return ResolveNestedValue(Model, pathInfo);
        }
        else
        {
            var indexerInfo = pathInfo[0].IndexerInfo;

            if (indexerInfo.HasIndexer)
            {
                propertyPath = indexerInfo.PathWithoutIndexer;
            }

            var nameFound = PropertyValues.ContainsKey(propertyPath);

            if (false == nameFound)
            {
                var allowImplicit = IsFlagSet(TemplateFlag.AllowImplicitBindings);

                if (allowImplicit)
                {
                    return null;
                }
                else
                {
                    throw new NettleRenderException
                    (
                        $"No property could be found with the name '{propertyPath}'."
                    );
                }
            }

            if (indexerInfo.HasIndexer)
            {
                var collection = PropertyValues[propertyPath];

                return ResolveIndexedBinding(propertyPath, collection, indexerInfo);
            }
            else
            {
                return PropertyValues[propertyPath];
            }
        }
    }

    /// <summary>
    /// Resolves a nested value from the path specified
    /// </summary>
    /// <param name="model">The model</param>
    /// <param name="path">The path info</param>
    /// <returns>The property value found</returns>
    private object? ResolveNestedValue(object? model, NettlePath path)
    {
        Validate.IsNotNull(path);

        if (model == null)
        {
            throw new NettleRenderException
            (
                $"Property {path} cannot be resolved because the model is null."
            );
        }

        var currentValue = model;

        // Try to resolve each segment one at a time until the end is reached
        foreach (var segment in path.Segments)
        {
            if (segment.IsModelPointer)
            {
                // Skip model pointer segments
                continue;
            }

            var segmentName = segment.Name;

            if (currentValue == null)
            {
                throw new NettleRenderException
                (
                    $"The path '{path}' contains a null reference at '{segmentName}'."
                );
            }

            currentValue = GetPropertyValue(currentValue, path, segmentName);

            if (segment.IndexerInfo.HasIndexer)
            {
                currentValue = ResolveIndexedBinding(path.FullPath, currentValue, segment.IndexerInfo);
            }
        }

        return currentValue;
    }

    /// <summary>
    /// Gets a property value from a model
    /// </summary>
    /// <param name="model">The model</param>
    /// <param name="path">The path info</param>
    /// <param name="propertyName">The property name</param>
    /// <returns>The matching property value</returns>
    /// <remarks>
    /// Models of type ExpandoObject are handled separately to 
    /// other object types. This is because ExpandoObject types 
    /// are used for managing anonymous types internally.
    /// </remarks>
    private object? GetPropertyValue(object model, NettlePath path, string propertyName)
    {
        bool propertyFound;
        var modelType = model.GetType();

        if (modelType == typeof(ExpandoObject))
        {
            var items = (IDictionary<string, object?>)model;

            propertyFound = items.ContainsKey(propertyName);
        }
        else
        {
            propertyFound = modelType.HasProperty(propertyName);
        }

        // Ensure the (or item entry) property exists first
        if (false == propertyFound)
        {
            var allowImplicit = IsFlagSet(TemplateFlag.AllowImplicitBindings);

            if (allowImplicit)
            {
                return null;
            }
            else
            {
                throw new NettleRenderException
                (
                    $"The path '{path}' does not contain a property named '{propertyName}'."
                );
            }
        }

        if (modelType == typeof(ExpandoObject))
        {
            var items = (IDictionary<string, object?>)model;

            return items[propertyName];
        }
        else
        {
            return modelType.GetProperty(propertyName)?.GetValue(model);
        }
    }

    /// <summary>
    /// Determines if a property path is a model reference
    /// </summary>
    /// <param name="path">The property path</param>
    /// <returns>True, if the path is a model reference; otherwise false</returns>
    /// <remarks>
    /// A model reference indicates that the entire model should be 
    /// issued instead of a specific property value.
    /// </remarks>
    public static bool IsModelReference(string path) => path?.Trim().Equals(@"$") ?? false;

    /// <summary>
    /// Gets the contexts variables
    /// </summary>
    public Dictionary<string, object?> Variables { get; private set; }

    /// <summary>
    /// Defines a variable for the template context
    /// </summary>
    /// <param name="name">The variable name</param>
    /// <param name="value">The variables value</param>
    internal void DefineVariable(string name, object? value)
    {
        Validate.IsNotEmpty(name);

        if (Variables.ContainsKey(name))
        {
            throw new InvalidOperationException
            (
                $"A variable with the name {name} has already been defined."
            );
        }
        else
        {
            Variables[name] = value;
        }
    }

    /// <summary>
    /// Reassigns the value of a variable for the template context
    /// </summary>
    /// <param name="name">The variable name</param>
    /// <param name="value">The variables value</param>
    internal void ReassignVariable(string name, object? value)
    {
        Validate.IsNotEmpty(name);

        if (false == Variables.ContainsKey(name))
        {
            throw new InvalidOperationException($"No variable was defined with the name {name}.");
        }

        var strictReassign = IsFlagSet(TemplateFlag.EnforceStrictReassign);

        if (strictReassign && value != null)
        {
            var currentValue = Variables[name];

            if (currentValue != null)
            {
                var oldValueType = currentValue.GetType();
                var newValueType = value.GetType();

                var isAssignable = oldValueType.IsAssignableFrom(newValueType);

                if (false == isAssignable)
                {
                    throw new InvalidOperationException
                    (
                        $"The type {newValueType.Name} cannot be assigned to type {oldValueType.Name}."
                    );
                }
            }
        }

        Variables[name] = value;

        Parent?.ReassignVariable(name, value);
    }

    /// <summary>
    /// Resolves a variable value from the variable path specified
    /// </summary>
    /// <param name="variablePath">The variable path</param>
    /// <returns>The variable value found</returns>
    public object? ResolveVariableValue(string variablePath)
    {
        Validate.IsNotEmpty(variablePath);

        var pathInfo = new NettlePath(variablePath);

        if (pathInfo.IsNested)
        {
            var variableName = pathInfo[0].Name;
            var variableValue = ResolveVariableValue(variableName);

            pathInfo.RemoveSegment(0);

            var nestedValue = ResolveNestedValue(variableValue, pathInfo);

            return nestedValue;
        }
        else
        {
            var indexerInfo = pathInfo[0].IndexerInfo;

            if (indexerInfo.HasIndexer)
            {
                variablePath = indexerInfo.PathWithoutIndexer;
            }

            var variableFound = Variables.ContainsKey(variablePath);

            if (false == variableFound)
            {
                var allowImplicit = IsFlagSet(TemplateFlag.AllowImplicitBindings);

                if (allowImplicit)
                {
                    return null;
                }
                else
                {
                    throw new NettleRenderException
                    (
                        $"No variable exists with the name {variablePath}."
                    );
                }
            }

            if (indexerInfo.HasIndexer)
            {
                var collection = Variables[variablePath];

                return ResolveIndexedBinding(variablePath, collection, indexerInfo);
            }
            else
            {
                return Variables[variablePath];
            }
        }
    }

    /// <summary>
    /// Resolves an indexed binding value
    /// </summary>
    /// <param name="bindingPath">The binding path</param>
    /// <param name="collection">The collection</param>
    /// <param name="indexer">The indexer information</param>
    /// <returns>The value found at the specified index</returns>
    private object ResolveIndexedBinding(string bindingPath, object? collection, Indexer indexer)
    {
        if (collection == null)
        {
            throw new InvalidOperationException
            (
                $"The value for '{bindingPath}' is null."
            );
        }

        if (false == collection.GetType().IsEnumerable())
        {
            throw new InvalidOperationException
            (
                $"'{bindingPath}' is not a valid collection."
            );
        }

        var index = ResolverIndexerValue(indexer);

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException
            (
                $"The index for '{bindingPath}' must be zero or greater."
            );
        }

        var counter = default(int);

        foreach (var item in (IEnumerable)collection)
        {
            if (counter == index)
            {
                if (indexer.NextIndexer != null)
                {
                    return ResolveIndexedBinding(indexer.FullPath, item, indexer.NextIndexer);
                }
                else
                {
                    return item;
                }
            }

            counter++;
        }

        throw new IndexOutOfRangeException
        (
            $"The index {index} for '{bindingPath}' is out of range."
        );
    }

    /// <summary>
    /// Resolves an indexer to a numeric value
    /// </summary>
    /// <param name="indexer">The indexer</param>
    /// <returns>The resolved value</returns>
    private int ResolverIndexerValue(Indexer indexer)
    {
        switch (indexer.IndexerValueType)
        {
            case NettleValueType.Number:
            {
                return indexer.ResolvedIndex;
            }
            case NettleValueType.Variable:
            {
                var value = ResolveVariableValue(indexer.IndexerSignature);

                if (value == null)
                {
                    throw new NullReferenceException
                    (
                        "The indexer must resolve to a numeric value."
                    );
                }

                if (value.GetType().IsNumeric())
                {
                    return Convert.ToInt32(value);
                }
                else
                {
                    throw new InvalidCastException
                    (
                        $"{value.GetType().Name} is not a valid indexer type."
                    );
                }
            }
            default:
            {
                return -1;
            }
        }
    }

    /// <summary>
    /// Creates a nested context that inherits from the current context
    /// </summary>
    /// <param name="model">The new model data</param>
    /// <returns>The template context created</returns>
    internal TemplateContext CreateNestedContext(object model)
    {
        var context = new TemplateContext(this, model, Flags);

        // Copy the partial call stack across
        foreach (var partial in PartialCallStack)
        {
            context.PartialCallStack.Add(partial);
        }

        var disableInheritance = IsFlagSet(TemplateFlag.DisableModelInheritance);

        if (false == disableInheritance)
        {
            // Migrate any properties that do not conflict with the new model
            foreach (var item in PropertyValues)
            {
                var propertyFound = context.PropertyValues.ContainsKey(item.Key);

                if (false == propertyFound)
                {
                    context.PropertyValues.Add(item.Key, item.Value);
                }
            }

            // Migrate the variables across
            foreach (var variable in Variables)
            {
                context.Variables.Add(variable.Key, variable.Value);
            }
        }

        return context;
    }
}
