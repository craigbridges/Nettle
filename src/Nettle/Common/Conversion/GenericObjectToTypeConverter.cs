namespace Nettle
{
    using System;

    /// <summary>
    /// Converter implementation for converting an object to a type value
    /// </summary>
    /// <typeparam name="T">The type to convert to</typeparam>
    public class GenericObjectToTypeConverter<T>
    {
        /// <summary>
        /// Converts an object value to a value of the type specified, if a conversion is possible
        /// </summary>
        /// <param name="value">The object value to convert</param>
        /// <returns>The converted value</returns>
        public T Convert(object value)
        {
            if (value == null)
            {
                return default(T);
            }
            else if (typeof(T) == value.GetType())
            {
                return (T)value;
            }
            else
            {
                var canConvert = value.GetType().CanConvert
                (
                    typeof(T),
                    value
                );

                if (canConvert)
                {
                    return (T)System.Convert.ChangeType
                    (
                        value,
                        typeof(T)
                    );
                }
                else if (value.GetType() == typeof(string))
                {
                    return ConvertFromString((string)value);
                }
                else
                {
                    RaiseCannotConvertException(value);

                    return default(T);
                }
            }
        }

        /// <summary>
        /// Converts a string value to the type specified
        /// </summary>
        /// <param name="value">The string value to convert</param>
        /// <returns>The converted value</returns>
        private T ConvertFromString(string value)
        {
            var convertedValue = default(object);
            var convertType = typeof(T);

            if (String.IsNullOrEmpty(value))
            {
                return default(T);
            }

            if (convertType.IsNullable())
            {
                convertType = Nullable.GetUnderlyingType(convertType);
            }

            if (convertType == typeof(DateTime))
            {
                convertedValue = System.Convert.ToDateTime(value);
            }
            else if (convertType == typeof(bool))
            {
                convertedValue = System.Convert.ToBoolean(value);
            }
            else if (convertType == typeof(double))
            {
                convertedValue = System.Convert.ToDouble(value);
            }
            else if (convertType == typeof(Single))
            {
                convertedValue = System.Convert.ToSingle(value);
            }
            else if (convertType == typeof(decimal))
            {
                convertedValue = System.Convert.ToDecimal(value);
            }
            else if (convertType == typeof(long))
            {
                convertedValue = System.Convert.ToInt64(value);
            }
            else if (convertType == typeof(int))
            {
                convertedValue = System.Convert.ToInt32(value);
            }
            else if (convertType == typeof(short))
            {
                convertedValue = System.Convert.ToInt16(value);
            }
            else if (convertType == typeof(char))
            {
                convertedValue = System.Convert.ToChar(value);
            }
            else if (convertType == typeof(byte))
            {
                convertedValue = System.Convert.ToByte(value);
            }
            else if (convertType.IsEnum)
            {
                convertedValue = Enum.Parse(convertType, value);
            }
            else
            {
                RaiseCannotConvertException(value);
            }

            return (T)convertedValue;
        }

        /// <summary>
        /// Raises an exception to indicate that the value could not be converted
        /// </summary>
        /// <param name="value">The value that could not be converted</param>
        private void RaiseCannotConvertException
            (
                object value
            )
        {
            var valueString = value.ToString();
            var typeName = typeof(T).ToString();

            var message = "The value '{0}' cannot be converted to the type '{1}'.";

            throw new InvalidCastException
            (
                message.With
                (
                    valueString,
                    typeName
                )
            );
        }
    }
}
