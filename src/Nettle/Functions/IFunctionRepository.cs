namespace Nettle.Functions
{
    /// <summary>
    /// Defines a contract for a repository that manages functions
    /// </summary>
    public interface IFunctionRepository
    {
        /// <summary>
        /// Adds a function to the repository
        /// </summary>
        /// <param name="function">The function to add</param>
        void AddFunction(IFunction function);

        /// <summary>
        /// Determines if a function exists with the name specified
        /// </summary>
        /// <param name="name">The function name</param>
        /// <returns>True, if the function exists; otherwise false</returns>
        bool FunctionExists(string name);

        /// <summary>
        /// Gets the function matching the name specified
        /// </summary>
        /// <param name="name">The function name</param>
        /// <returns>The matching function</returns>
        IFunction GetFunction(string name);

        /// <summary>
        /// Gets a collection of all functions in the repository
        /// </summary>
        /// <returns>A collection of matching functions</returns>
        IEnumerable<IFunction> GetAllFunctions();
    }
}
