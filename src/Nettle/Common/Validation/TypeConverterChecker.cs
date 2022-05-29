namespace Nettle
{
    /// <summary>
    /// Used for checking if an object of type TFrom can be converted to the type TTo
    /// </summary>
    /// <typeparam name="TFrom">The convert from type</typeparam>
    /// <typeparam name="TTo">The convert to type</typeparam>
    public class TypeConverterChecker<TFrom, TTo>
    {
        public bool CanConvert { get; private set; }

        /// <summary>
        /// Determines if an object can be converted to the type specified
        /// </summary>
        /// <param name="from">The object to be converted</param>
        public TypeConverterChecker(TFrom from)
        {
            CanConvert = false;

            // Check for string to numeric type conversions first
            if (typeof(TFrom) == typeof(string) && typeof(TTo).IsNumeric())
            {
                if (typeof(TFrom).IsNumeric())
                {
                    try
                    {
                        Convert.ChangeType(from, typeof(TTo));
                        CanConvert = true;
                    }
                    catch { }
                }
            }
            else
            {
                try
                {
                    var to = (TTo)(dynamic)from!;
                    CanConvert = true;
                }
                catch { }
            }
        }
    }
}
