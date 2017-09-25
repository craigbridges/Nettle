namespace Nettle
{
    using System;

    /// <summary>
    /// Represents various extension methods for the Double class
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Determines if the double represents a whole number
        /// </summary>
        /// <param name="x">The number to check</param>
        /// <returns>True, if it is a whole number; otherwise false</returns>
        public static bool IsWholeNumber
            (
                this double x
            )
        {
            return Math.Abs(x % 1) < Double.Epsilon;
        }
    }
}
