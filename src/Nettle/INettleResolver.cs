namespace Nettle
{
    using Nettle.Functions;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a contract for a Nettle dependency resolver
    /// </summary>
    public interface INettleResolver
    {
        /// <summary>
        /// Resolves a collection of all functions that can be resolved
        /// </summary>
        /// <returns>A collection of matching functions</returns>
        IEnumerable<IFunction> ResolveFunctions();
    }
}
