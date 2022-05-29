namespace Nettle.Compiler.Parsing
{
    /// <summary>
    /// Represents a string tokenizer
    /// </summary>
    internal sealed class Tokenizer
    {
        public Tokenizer(char separator = ' ')
        {
            Separator = separator;

            Enclosures = new Dictionary<char, char>()
            {
                { '"', '"' },
                { '(', ')' },
                { '<', '>' }
            };
        }

        /// <summary>
        /// Gets the token separator character
        /// </summary>
        public char Separator { get; private set; }
        
        /// <summary>
        /// Gets a dictionary of enclosure characters
        /// </summary>
        /// <remarks>
        /// Enclosure characters specify how tokens can be wrapped.
        /// 
        /// Each enclosure pair has an opening character and 
        /// a closing character. When the tokenizer scans through
        /// a string, if an opening enclosure character is found,
        /// the token is not completed until a closing character 
        /// is also found or the end of the string is reached.
        /// </remarks>
        public Dictionary<char, char> Enclosures { get; private set; }

        /// <summary>
        /// Splits the value into individual tokens
        /// </summary>
        /// <param name="value">The value to tokenize</param>
        /// <returns>An array of tokens</returns>
        public string[] Tokenize(string value)
        {
            // Remove extra white space before tokenizing
            value = value.Trim().Replace("  ", " ");

            var tokens = new List<string>();
            var tokenBuilder = new StringBuilder();
            var newToken = true;
            var enclosures = Enclosures;
            var enclosureQueue = new Stack<char>();
            var index = 0;

            foreach (var c in value)
            {
                if (newToken)
                {
                    // Start a new token when the builder is empty
                    if (c != Separator)
                    {
                        tokenBuilder.Append(c);
                    }

                    if (enclosures.ContainsKey(c))
                    {
                        var containsClosing = value[index..].Contains(enclosures[c].ToString());

                        // Only enqueue the enclosure if the remaining string has it
                        if (containsClosing)
                        {
                            enclosureQueue.Push(enclosures[c]);
                        }
                        else
                        {
                            enclosureQueue.Push(Separator);
                        }
                    }
                    else
                    {
                        enclosureQueue.Push(Separator);
                    }

                    newToken = false;
                }
                else
                {
                    var tokenComplete = false;

                    // Check if this character is an ending enclosure
                    if (enclosureQueue.Any() && enclosureQueue.Peek() == c)
                    {
                        enclosureQueue.Pop();

                        if (enclosureQueue.Count == 0)
                        {
                            tokenComplete = true;
                        }
                    }
                    else if (enclosures.ContainsKey(c))
                    {
                        var containsClosing = value[index..].Contains(enclosures[c].ToString());

                        // Only enqueue the enclosure if the remaining string has it
                        if (containsClosing)
                        {
                            enclosureQueue.Push(enclosures[c]);
                        }
                    }

                    if (false == tokenComplete || c != Separator)
                    {
                        tokenBuilder.Append(c);
                    }

                    // Check if we should flush the current token
                    if (tokenComplete)
                    {
                        tokens.Add(tokenBuilder.ToString().Trim());

                        tokenBuilder.Clear();
                        newToken = true;
                    }
                }

                index++;
            }

            if (tokenBuilder.Length > 0)
            {
                tokens.Add(tokenBuilder.ToString().Trim());
            }

            return tokens.ToArray();
        }
    }
}
