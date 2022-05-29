namespace Nettle.Functions
{
    public sealed class FunctionRepository : IFunctionRepository
    {
        private readonly Dictionary<string, IFunction> _functions;

        public FunctionRepository(params INettleResolver[] resolvers)
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

        public void AddFunction(IFunction function)
        {
            Validate.IsNotNull(function);

            var name = function.Name;
            var found = _functions.ContainsKey(name);

            if (found)
            {
                throw new InvalidOperationException
                (
                    $"A function with the name '{name}' has already been added."
                );
            }

            _functions.Add(name, function);
        }

        public bool FunctionExists(string name)
        {
            Validate.IsNotEmpty(name);

            return _functions.ContainsKey(name);
        }

        public IFunction GetFunction(string name)
        {
            Validate.IsNotEmpty(name);

            var found = _functions.ContainsKey(name);

            if (false == found)
            {
                throw new KeyNotFoundException
                (
                    $"No function was found matching the name '{name}'."
                );
            }

            return _functions[name];
        }

        public IEnumerable<IFunction> GetAllFunctions()
        {
            return _functions.Select(pair => pair.Value).OrderBy(x => x.Name);
        }
    }
}
