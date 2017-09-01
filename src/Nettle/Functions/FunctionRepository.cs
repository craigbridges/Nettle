namespace Nettle.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a simple implementation of a function repository
    /// </summary>
    public sealed class FunctionRepository : IFunctionRepository
    {
        private Dictionary<string, IFunction> _functions;

        /// <summary>
        /// Constructs the repository by auto resolving all functions
        /// </summary>
        /// <param name="resolvers">The resolvers</param>
        public FunctionRepository
            (
                params INettleResolver[] resolvers
            )
        {
            Validate.IsNotNull(resolvers);

            BuildFunctionsDictionary(resolvers);
        }

        /// <summary>
        /// Builds a dictionary of functions using reflection
        /// </summary>
        /// <param name="resolvers">The resolvers</param>
        private void BuildFunctionsDictionary
            (
                params INettleResolver[] resolvers
            )
        {
            Validate.IsNotNull(resolvers);
            
            _functions = new Dictionary<string, IFunction>();

            foreach (var resolver in resolvers)
            {
                var resolvedFunctions = resolver.ResolveFunctions();

                foreach (var function in resolvedFunctions)
                {
                    var name = function.Name;

                    _functions[name] = function;
                }
            }
        }

        /// <summary>
        /// Adds a function to the repository
        /// </summary>
        /// <param name="function">The function to add</param>
        public void AddFunction
            (
                IFunction function
            )
        {
            Validate.IsNotNull(function);

            var name = function.Name;
            var found = _functions.ContainsKey(name);

            if (found)
            {
                throw new InvalidOperationException
                (
                    "A function with the name '{0}' has already been added.".With
                    (
                        name
                    )
                );
            }

            _functions.Add(name, function);
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
