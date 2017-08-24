namespace Nettle
{
    using Nettle.Compiler;
    using System;

    /// <summary>
    /// Represents the entry point for all Nettle actions
    /// </summary>
    public static class NettleEngine
    {
        /// <summary>
        /// Gets a Nettle compiler instance
        /// </summary>
        /// <returns>The compiler</returns>
        public static INettleCompiler GetCompiler()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a Nettle compiler using the activator specified
        /// </summary>
        /// <param name="activator">The activator</param>
        /// <returns>The compiler</returns>
        public static INettleCompiler GetCompiler
            (
                INettleActivator activator
            )
        {
            throw new NotImplementedException();
        }
    }
}
