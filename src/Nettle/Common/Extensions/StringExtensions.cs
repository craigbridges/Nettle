namespace Nettle
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// Provides various extension methods for the String class
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Truncates a given string to the number of characters specified by the length value
        /// </summary>
        /// <param name="input">The string to truncate</param>
        /// <param name="length">The length to reduce to</param>
        /// <returns>The truncated string</returns>
        public static string Truncate
            (
                this string input,
                int length
            )
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }
            else
            {
                var allowedLength = input.Length <= length;

                return 
                (
                    allowedLength ? input : input.Substring(0, length)
                );
            }
        }

        /// <summary>
        /// Crops a string by removing the characters preceding a start index
        /// </summary>
        /// <param name="value">The value to crop</param>
        /// <param name="startIndex">The start index</param>
        /// <returns>The cropped string</returns>
        public static string Crop
            (
                this string value,
                int startIndex
            )
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                var endIndex = default(int);
                
                if (startIndex == value.Length)
                {
                    endIndex = startIndex;
                }
                else
                {
                    endIndex = value.Length - 1;
                }

                return Crop
                (
                    value,
                    startIndex,
                    endIndex
                );
            }
        }

        /// <summary>
        /// Crops a string by removing the characters between a two indexes
        /// </summary>
        /// <param name="value">The value to crop</param>
        /// <param name="startIndex">The start index</param>
        /// <param name="endIndex">The end index</param>
        /// <returns>The cropped string</returns>
        public static string Crop
            (
                this string value,
                int startIndex,
                int endIndex
            )
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                if (startIndex < 0 || endIndex < 0)
                {
                    throw new ArgumentOutOfRangeException
                    (
                        "The index values cannot be less than zero."
                    );
                }

                if (startIndex > value.Length)
                {
                    throw new IndexOutOfRangeException
                    (
                        "The start index must be before the end of the string."
                    );
                }

                if (endIndex > value.Length)
                {
                    throw new IndexOutOfRangeException
                    (
                        "The end index must be before the end of the string."
                    );
                }

                if (startIndex > endIndex)
                {
                    throw new ArgumentException
                    (
                        "The start index must be smaller than the end index."
                    );
                }

                if (startIndex == value.Length)
                {
                    return String.Empty;
                }
                else
                {
                    var length = (endIndex - startIndex) + 1;

                    if (length == 0)
                    {
                        length = 1;
                    }

                    return value.Substring
                    (
                        startIndex,
                        length
                    );
                }
            }
        }

        /// <summary>
        /// Shortcut syntax sugar for String.Format() that just requires the args values
        /// </summary>
        /// <param name="value">The string to format</param>
        /// <param name="args">The args values to merge into the string</param>
        /// <returns>The formatted string</returns>
        public static string With
            (
                this string value,
                params object[] args
            )
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                return String.Format(value, args);
            }
        }

        /// <summary>
        /// Use the current thread's culture info for conversion
        /// </summary>
        public static string ToTitleCase
            (
                this string value
            )
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                var cultureInfo = Thread.CurrentThread.CurrentCulture;

                return cultureInfo.TextInfo.ToTitleCase
                (
                    value.ToLower()
                );
            }
        }

        /// <summary>
        /// Overload which uses the culture info with the specified name
        /// </summary>
        public static string ToTitleCase
            (
                this string value,
                string cultureInfoName
            )
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                var cultureInfo = new CultureInfo(cultureInfoName);

                return cultureInfo.TextInfo.ToTitleCase
                (
                    value.ToLower()
                );
            }
        }

        /// <summary>
        /// Overload which uses the specified culture info
        /// </summary>
        public static string ToTitleCase
            (
                this string value,
                CultureInfo cultureInfo
            )
        {
            Validate.IsNotNull(cultureInfo);

            if (value == null)
            {
                return value;
            }
            else
            {
                return cultureInfo.TextInfo.ToTitleCase
                (
                    value.ToLower()
                );
            }
        }

        /// <summary>
        /// Determines if the string has a value
        /// </summary>
        public static bool HasValue
            (
                this string value
            )
        {
            return 
            (
                false == String.IsNullOrEmpty(value) && value.Trim().Length > 0
            );
        }

        /// <summary>
        /// Determines if one string has the same value as another
        /// </summary>
        public static bool IsEqualTo
            (
                this string value,
                string other
            )
        {
            return value.Equals
            (
                other,
                StringComparison.OrdinalIgnoreCase
            );
        }

        /// <summary>
        /// Determines if the given string contains non-printable (control) characters
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True, if the string contains non-printable characters; otherwise false</returns>
        public static bool ContainsNonPrintableCharacters
            (
                this string value
            )
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }
            else
            {
                return value.Any
                (
                    c => Char.IsControl(c)
                );
            }
        }

        /// <summary>
        /// Determines if the string specified contains any non-ASCII characters
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True, if the string is ASCII only; otherwise false</returns>
        /// <remarks>
        /// ASCII encoding replaces non-ASCII with question marks, so we use UTF8 to see 
        /// if multi-byte sequences are there.
        /// </remarks>
        public static bool ContainsNonAscii
            (
                this string value
            )
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }
            else
            {
                return Encoding.UTF8.GetByteCount(value) != value.Length;
            }
        }
        
        /// <summary>
        /// Removes all special characters from the string value
        /// </summary>
        /// <param name="value">The value to remove special characters from</param>
        /// <returns>The string without special characters</returns>
        /// <remarks>
        /// See http://stackoverflow.com/a/16725861
        /// </remarks>
        public static string RemoveSpecialCharacters
            (
                this string value
            )
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                var sb = new StringBuilder();

                foreach (char c in value)
                {
                    if (Char.IsLetterOrDigit(c) || Char.IsSymbol(c) || Char.IsWhiteSpace(c) || Char.IsPunctuation(c))
                    {
                        sb.Append(c);
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Returns the first string with a non-empty non-null value
        /// </summary>
        /// <param name="input">The input value</param>
        /// <param name="alternative">The alternative value</param>
        /// <returns>The first string with a non-empty non-null value</returns>
        public static string Or
            (
                this string input,
                string alternative
            )
        {
            return 
            (
                (String.IsNullOrEmpty(input) == false) ? input : alternative
            );
        }

        /// <summary>
        /// Determines if the pattern is like the text using a wildcard match
        /// </summary>
        /// <param name="pattern">The pattern containing the wildcards</param>
        /// <param name="text">The text to match against</param>
        /// <param name="caseSensitive">If true, a case sensitive match is performed</param>
        /// <returns>True, if the pattern matches the text</returns>
        public static bool IsLike
            (
                this string pattern,
                string text,
                bool caseSensitive = false
            )
        {
            Contract.Requires(false == String.IsNullOrEmpty(pattern));

            pattern = pattern.Replace(".", @"\.");
            pattern = pattern.Replace("?", ".");
            pattern = pattern.Replace("*", ".*?");
            pattern = pattern.Replace(@"\", @"\\");
            pattern = pattern.Replace(" ", @"\s");

            var regex = new Regex
            (
                pattern,
                caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase
            );

            return regex.IsMatch
            (
                text
            );
        }

        /// <summary>
        /// Determines if a string value is numeric
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True, if the string is numeric; otherwise false</returns>
        public static bool IsNumeric
            (
                this string value
            )
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            else
            {
                return Double.TryParse
                (
                    value,
                    out double number
                );
            }
        }

        /// <summary>
        /// Determines if a string contains only alpha numeric characters
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True, if the string contains only alpha numeric; otherwise false</returns>
        public static bool IsAlphaNumeric
            (
                this string value
            )
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            else
            {
                return value.All
                (
                    Char.IsLetterOrDigit
                );
            }
        }

        /// <summary>
        /// Gets the line number from the position specified in a string
        /// </summary>
        /// <param name="value">The string value to get the line number from</param>
        /// <param name="position">The position to get the line number for</param>
        /// <returns>The line number found</returns>
        public static int LineFromPosition
            (
                this string value,
                int position
            )
        {
            Validate.IsNotEmpty(value);

            var lineNumber = 1;

            if (position > value.Length) position = value.Length;

            for (int i = 0; i <= position - 1; i++)
            {
                if (value[i] == '\n') lineNumber++;
            }

            return lineNumber;
        }

        /// <summary>
        /// Finds all instances of a substring within a string and returns the indexes of each occurrence
        /// </summary>
        /// <param name="haystack">The string to search</param>
        /// <param name="needle">The string to search for</param>
        /// <returns>An enumeration of all indexes indicating where the substring was found</returns>
        public static IEnumerable<int> IndexesOf
            (
                this string haystack,
                string needle
            )
        {
            Validate.IsNotEmpty(haystack);

            int lastIndex = 0;

            while (true)
            {
                int index = haystack.IndexOf(needle, lastIndex);

                if (index == -1)
                {
                    yield break;
                }

                yield return index;

                lastIndex = index + needle.Length;
            }
        }

        /// <summary>
        /// Counts the number of occurrences of a substring within the string specified
        /// </summary>
        /// <param name="input">The input string to search</param>
        /// <param name="search">The search string</param>
        /// <returns>The number of occurrences found</returns>
        public static int Count
            (
                this string input,
                string search
            )
        {
            Validate.IsNotEmpty(input);

            var indexes = input.IndexesOf(search);

            return
            (
                indexes == null ? 0 : indexes.Count()
            );
        }

        /// <summary>
        /// Extracts the string found to the left of the string value specified
        /// </summary>
        /// <param name="input">The string to search</param>
        /// <param name="value">The matching value</param>
        /// <returns>The string found to the left of the value specified</returns>
        public static string LeftOf
            (
                this string input,
                string value
            )
        {
            Validate.IsNotEmpty(input);

            var firstIndex = input.IndexOf(value);

            if (firstIndex < 1)
            {
                return String.Empty;
            }
            else
            {
                return input.Substring
                (
                    0,
                    firstIndex
                );
            }
        }

        /// <summary>
        /// Extracts the string found to the right of the string value specified
        /// </summary>
        /// <param name="input">The string to search</param>
        /// <param name="value">The matching value</param>
        /// <returns>The string found to the right of the value specified</returns>
        public static string RightOf
            (
                this string input,
                string value
            )
        {
            Validate.IsNotEmpty(input);

            var lastIndex = input.LastIndexOf(value);

            if ((lastIndex + value.Length) == input.Length)
            {
                return String.Empty;
            }
            else
            {
                var startIndex = 
                (
                    lastIndex + value.Length
                );

                return input.Substring(startIndex);
            }
        }
    }
}
