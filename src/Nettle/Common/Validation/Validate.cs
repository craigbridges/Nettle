namespace Nettle
{
    using System;

    /// <summary>
    /// Provides various static parameter validation methods
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// Checks that an object value is not null
        /// </summary>
        /// <param name="o">The value to check</param>
        /// <param name="paramName">The parameter name (optional)</param>
        public static void IsNotNull
            (
                object o,
                string paramName = null
            )
        {
            if (o == null)
            {
                if (String.IsNullOrEmpty(paramName))
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    throw new ArgumentNullException
                    (
                        paramName
                    );
                }
            }
        }

        /// <summary>
        /// Ensures a string has a value (i.e. it is not null or empty)
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <param name="paramName">The parameter name (optional)</param>
        public static void IsNotEmpty
            (
                string input,
                string paramName = null
            )
        {
            if (String.IsNullOrEmpty(input))
            {
                if (String.IsNullOrEmpty(paramName))
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    throw new ArgumentNullException
                    (
                        paramName
                    );
                }
            }
        }
    }
}
