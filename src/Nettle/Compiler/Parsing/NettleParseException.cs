namespace Nettle.Compiler.Parsing
{
    using System;

    /// <summary>
    /// Represents a Nettle parse exception
    /// </summary>
    [Serializable]
    public class NettleParseException : Exception
    {
        internal NettleParseException
            (
                string message,
                int position
            )

            : base(message)
        {
            this.Position = position;
        }

        internal NettleParseException
            (
                string message,
                int position,
                Exception innerException
            )

            : base(message, innerException)
        {
            this.Position = position;
        }

        /// <summary>
        /// Gets the character position where the error was found
        /// </summary>
        public int Position { get; private set; }
    }
}
