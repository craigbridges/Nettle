namespace Nettle.Compiler
{
    /// <summary>
    /// Represents a Nettle render exception
    /// </summary>
    [Serializable]
    public class NettleRenderException : Exception
    {
        internal NettleRenderException(string message)
            : base(message)
        { }

        internal NettleRenderException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
