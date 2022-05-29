namespace Nettle;

public static class StringExtensions
{
    /// <summary>
    /// Truncates a given string to the number of characters specified by the length value
    /// </summary>
    /// <param name="input">The string to truncate</param>
    /// <param name="length">The length to reduce to</param>
    /// <returns>The truncated string</returns>
    public static string Truncate(this string input, int length)
    {
        if (String.IsNullOrEmpty(input))
        {
            return input;
        }
        else
        {
            return input.Length <= length ? input : input[..length];
        }
    }

    /// <summary>
    /// Crops a string by removing the characters preceding a start index
    /// </summary>
    /// <param name="value">The value to crop</param>
    /// <param name="startIndex">The start index</param>
    /// <returns>The cropped string</returns>
    public static string Crop(this string value, int startIndex)
    {
        if (String.IsNullOrEmpty(value))
        {
            return value;
        }
        else
        {
            int endIndex;

            if (startIndex == value.Length)
            {
                endIndex = startIndex;
            }
            else
            {
                endIndex = value.Length - 1;
            }

            return Crop(value, startIndex, endIndex);
        }
    }

    /// <summary>
    /// Crops a string by removing the characters between a two indexes
    /// </summary>
    /// <param name="value">The value to crop</param>
    /// <param name="startIndex">The start index</param>
    /// <param name="endIndex">The end index</param>
    /// <returns>The cropped string</returns>
    public static string Crop(this string value, int startIndex, int endIndex)
    {
        if (String.IsNullOrEmpty(value))
        {
            return value;
        }
        else
        {
            if (startIndex < 0 || endIndex < 0)
            {
                throw new ArgumentOutOfRangeException("The index values cannot be less than zero.");
            }

            if (startIndex > value.Length)
            {
                throw new IndexOutOfRangeException("The start index must be before the end of the string.");
            }

            if (endIndex > value.Length)
            {
                throw new IndexOutOfRangeException("The end index must be before the end of the string.");
            }

            if (startIndex > endIndex)
            {
                throw new ArgumentException("The start index must be smaller than the end index.");
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

                return value.Substring(startIndex, length);
            }
        }
    }

    /// <summary>
    /// Use the current thread's culture info for conversion
    /// </summary>
    public static string ToTitleCase(this string value)
    {
        if (String.IsNullOrEmpty(value))
        {
            return value;
        }
        else
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;

            return cultureInfo.TextInfo.ToTitleCase(value.ToLower());
        }
    }

    /// <summary>
    /// Determines if a string value is numeric
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <returns>True, if the string is numeric; otherwise false</returns>
    public static bool IsNumeric(this string value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        else
        {
            return Double.TryParse(value, out double _);
        }
    }

    /// <summary>
    /// Determines if a string contains only alpha numeric characters
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <returns>True, if the string contains only alpha numeric; otherwise false</returns>
    public static bool IsAlphaNumeric(this string value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        else
        {
            return value.All(Char.IsLetterOrDigit);
        }
    }

    /// <summary>
    /// Determines if a string constitutes a valid C# property name
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <returns>True, if valid property name; otherwise false</returns>
    public static bool IsValidPropertyName(this string value)
    {
        const string PropertyPattern = @"^@?[a-zA-Z_]\w*(\.@?[a-zA-Z_]\w*)*$";

        if (String.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        else
        {
            return Regex.IsMatch(value, PropertyPattern, RegexOptions.Compiled);
        }
    }

    /// <summary>
    /// Determines if a string is made up of any of the characters in an array
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <param name="matchValues">An array of match values</param>
    /// <returns>True, if a match was found; otherwise false</returns>
    public static bool IsMadeUpOf(this string value, params char[] matchValues)
    {
        if (String.IsNullOrEmpty(value))
        {
            return false;
        }
        else
        {
            return value.All(c => matchValues.Any(mv => mv == c));
        }
    }

    /// <summary>
    /// Determines if a string starts with any values in an array
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <param name="matchValues">An array of match values</param>
    /// <returns>True, if a match was found; otherwise false</returns>
    public static bool StartsWithAny(this string value, params string[] matchValues)
    {
        if (String.IsNullOrEmpty(value))
        {
            return false;
        }
        else
        {
            foreach (var matchValue in matchValues)
            {
                if (value.StartsWith(matchValue))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Determines if a string ends with any values in an array
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <param name="matchValues">An array of match values</param>
    /// <returns>True, if a match was found; otherwise false</returns>
    public static bool EndsWithAny(this string value, params string[] matchValues)
    {
        if (String.IsNullOrEmpty(value))
        {
            return false;
        }
        else
        {
            foreach (var matchValue in matchValues)
            {
                if (value.EndsWith(matchValue))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Extracts the string found to the left of the string value specified
    /// </summary>
    /// <param name="input">The string to search</param>
    /// <param name="value">The matching value</param>
    /// <returns>The string found to the left of the value specified</returns>
    public static string LeftOf(this string input, string value)
    {
        Validate.IsNotEmpty(input);

        var firstIndex = input.IndexOf(value);

        if (firstIndex < 1)
        {
            return String.Empty;
        }
        else
        {
            return input[..firstIndex];
        }
    }

    /// <summary>
    /// Extracts the string found to the right of the string value specified
    /// </summary>
    /// <param name="input">The string to search</param>
    /// <param name="value">The matching value</param>
    /// <returns>The string found to the right of the value specified</returns>
    public static string RightOf(this string input, string value)
    {
        Validate.IsNotEmpty(input);

        var lastIndex = input.LastIndexOf(value);

        if ((lastIndex + value.Length) == input.Length)
        {
            return String.Empty;
        }
        else
        {
            var startIndex = (lastIndex + value.Length);

            return input[startIndex..];
        }
    }

    /// <summary>
    /// Replaces the first occurrence of a string within another string
    /// </summary>
    /// <param name="text">The text to search</param>
    /// <param name="search">The search value</param>
    /// <param name="replace">The replace value</param>
    /// <returns>The updated string</returns>
    public static string ReplaceFirst(this string text, string search, string replace)
    {
        var position = text.IndexOf(search);

        if (position < 0)
        {
            return text;
        }
        else
        {
            var leftValue = text[..position];
            var rightValue = text[(position + search.Length)..];

            return leftValue + replace + rightValue;
        }
    }

    /// <summary>
    /// Replaces the last occurrence of a string within another string
    /// </summary>
    /// <param name="text">The text to search</param>
    /// <param name="search">The search value</param>
    /// <param name="replace">The replace value</param>
    /// <returns>The updated string</returns>
    public static string ReplaceLast(this string text, string search, string replace)
    {
        var position = text.LastIndexOf(search);

        if (position < 0)
        {
            return text;
        }
        else
        {
            var leftValue = text[..position];
            var rightValue = text[(position + search.Length)..];

            return leftValue + replace + rightValue;
        }
    }

    /// <summary>
    /// Removes the first occurrence of a string within another string
    /// </summary>
    /// <param name="text">The text to search</param>
    /// <param name="search">The search value</param>
    /// <returns>The updated string</returns>
    public static string RemoveFirst(this string text, string search)
    {
        return ReplaceFirst(text, search, String.Empty);
    }

    /// <summary>
    /// Removes the last occurrence of a string within another string
    /// </summary>
    /// <param name="text">The text to search</param>
    /// <param name="search">The search value</param>
    /// <returns>The updated string</returns>
    public static string RemoveLast(this string text, string search)
    {
        return ReplaceLast(text, search, String.Empty);
    }

    /// <summary>
    /// Removes all leading and trailing phrases specified in an array from a string
    /// </summary>
    /// <param name="text">The text</param>
    /// <param name="phases">The search phases</param>
    /// <returns>The updated text</returns>
    public static string Trim(this string text, params string[] phases)
    {
        if (String.IsNullOrEmpty(text))
        {
            return text;
        }

        bool phrasesFound;

        do
        {
            phrasesFound = false;

            foreach (var phrase in phases)
            {
                if (text.StartsWith(phrase))
                {
                    phrasesFound = true;
                    text = text.RemoveFirst(phrase);
                }

                if (text.EndsWith(phrase))
                {
                    phrasesFound = true;
                    text = text.RemoveLast(phrase);
                }
            }
        }
        while (phrasesFound);

        return text;
    }
}
