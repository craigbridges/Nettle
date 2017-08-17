namespace Nettle.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a simple implementation of a function repository
    /// </summary>
    public class SimpleFunctionRepository : IFunctionRepository
    {
        private Dictionary<string, IFunction> _functions;

        /// <summary>
        /// Constructs the repository by auto resolving all functions
        /// </summary>
        public SimpleFunctionRepository()
        {
            BuildFunctionsDictionary();
        }

        /// <summary>
        /// Builds a dictionary of functions using reflection
        /// </summary>
        private void BuildFunctionsDictionary()
        {
            _functions = new Dictionary<string, IFunction>();

            var interfaceType = typeof(IFunction);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var allTypes = assemblies.SelectMany
            (
                s => s.GetTypes()
            );

            var typesFound = allTypes.Where
            (
                p => interfaceType.IsAssignableFrom(p)
            );

            foreach (var type in typesFound)
            {
                var constructor = type.GetConstructor
                (
                    Type.EmptyTypes
                );

                if (constructor == null)
                {
                    throw new InvalidOperationException
                    (
                        "Could not resolve type {0}. An empty constructor is required.".With
                        (
                            type.Name
                        )
                    );
                }

                var functionInstance = (IFunction)Activator.CreateInstance
                (
                    type
                );

                var name = functionInstance.Name;

                _functions[name] = functionInstance;
            }
        }

        /// <summary>
        /// Determines if a function exists with the name specified
        /// </summary>
        /// <param name="name">The function name</param>
        /// <returns>True, if the function exists; otherwise false</returns>
        public bool FunctionExists
            (
                string name
            )
        {
            Validate.IsNotEmpty(name);

            return _functions.ContainsKey
            (
                name
            );
        }

        /// <summary>
        /// Gets the function matching the name specified
        /// </summary>
        /// <param name="name">The function name</param>
        /// <returns>The matching function</returns>
        public IFunction GetFunction
            (
                string name
            )
        {
            Validate.IsNotEmpty(name);

            var found = _functions.ContainsKey
            (
                name
            );

            if (false == found)
            {
                throw new KeyNotFoundException
                (
                    "No function was found matching the name '{0}'.".With
                    (
                        name
                    )
                );
            }

            return _functions[name];
        }

        /// <summary>
        /// Gets a collection of all functions in the repository
        /// </summary>
        /// <returns>A collection of matching functions</returns>
        public IEnumerable<IFunction> GetAllFunctions()
        {
            var allFunctions = _functions.Select
            (
                f => f.Value
            );

            return allFunctions.OrderBy
            (
                a => a.Name
            );
        }
    }
}
