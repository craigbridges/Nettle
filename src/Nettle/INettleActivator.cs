namespace Nettle
{
    using System;

    /// <summary>
    /// Defines a contract for a Nettle activator
    /// </summary>
    public interface INettleActivator
    {
        /// <summary>
        /// Creates an instance of the type specified
        /// </summary>
        /// <param name="type">The object type</param>
        /// <returns>The object instance</returns>
        object CreateInstance(Type type);
    }
}
