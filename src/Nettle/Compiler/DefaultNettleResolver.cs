namespace Nettle.Compiler
{
    using Nettle.Functions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Represents a default Nettle resolver implementation
    /// </summary>
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
            var assembly = this.GetType().Assembly;

            var typesFound = assembly.GetTypes().Where
            (
                p => interfaceType.IsAssignableFrom(p)
                    && false == p.IsAbstract
                    && false == p.IsInterface
            );

            foreach (var type in typesFound)
            {
                var constructor = type.GetConstructor
                (
                    Type.EmptyTypes
                );

                if (constructor != null)
                {
                    var functionInstance = (IFunction)Activator.CreateInstance
                    (
                        type
                    );

                    functions.Add(functionInstance);
                }
                else
                {
                    Debug.WriteLine
                    (
                        "Warning: The type {0} could not be resolved.".With
                        (
                            type.Name
                        )
                    );
                }
            }

            return functions;
        }
    }
}
