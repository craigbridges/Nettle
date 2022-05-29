namespace Nettle.Compiler;

public class DefaultNettleResolver : INettleResolver
{
    /// <summary>
    /// Resolves a collection of all functions that can be resolved
    /// </summary>
    /// <returns>A collection of matching functions</returns>
    public virtual IEnumerable<IFunction> ResolveFunctions()
    {
        var functions = new List<IFunction>();
        var interfaceType = typeof(IFunction);
        var assembly = GetType().Assembly;

        var typesFound = assembly
            .GetTypes()
            .Where(t => interfaceType.IsAssignableFrom(t) && false == t.IsAbstract && false == t.IsInterface);

        foreach (var type in typesFound)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor != null)
            {
                var functionInstance = (IFunction?)Activator.CreateInstance(type);

                if (functionInstance != null)
                {
                    functions.Add(functionInstance);
                }
            }
            else
            {
                Debug.WriteLine($"Warning: The type {type.Name} could not be resolved.");
            }
        }

        return functions;
    }
}
