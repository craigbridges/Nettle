namespace Nettle.Compiler
{
    using System;

    /// <summary>
    /// Represents a Nettle render exception
    /// </summary>
    public class NettleRenderException : Exception
    {
        internal NettleRenderException
            (
                string message
            )

            : base(message)
        { }

        internal NettleRenderException
            (
                string message,
                Exception innerException
            )

            : base(message, innerException)
        { }
    }
}
