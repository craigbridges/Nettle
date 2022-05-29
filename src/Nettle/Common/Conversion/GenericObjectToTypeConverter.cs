namespace Nettle;

/// <summary>
/// Generic converter for converting an object to a specific type
/// </summary>
/// <typeparam name="T">The type to convert to</typeparam>
public class GenericObjectToTypeConverter<T>
{
    /// <summary>
    /// Converts an object value to a value of the type specified, if a conversion is possible
    /// </summary>
    /// <param name="value">The object value to convert</param>
    /// <returns>The converted value</returns>
    public T? Convert(object? value)
    {
        if (value == null)
        {
            return default;
        }

        var valueType = value.GetType();

        if (typeof(T) == valueType || valueType.IsSubclassOf(typeof(T)))
        {
            return (T)value;
        }
        else
        {
            var canConvert = valueType.CanConvert(typeof(T), value);

            if (canConvert)
            {
                var type = typeof(T);

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (type != null)
                {
                    return (T)System.Convert.ChangeType(value, type);
                }
                else
                {
                    return default;
                }
            }
            else if (value.GetType() == typeof(string))
            {
                return ConvertFromString((string)value);
            }
            else
            {
                GenericObjectToTypeConverter<T>.RaiseCannotConvertException(value);

                return default;
            }
        }
    }

    /// <summary>
    /// Converts a string value to the type specified
    /// </summary>
    /// <param name="value">The string value to convert</param>
    /// <returns>The converted value</returns>
    private T? ConvertFromString(string? value)
    {
        var convertedValue = default(object);
        var convertType = typeof(T);

        if (String.IsNullOrEmpty(value))
        {
            return default;
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
        else if (convertType != null && convertType.IsEnum)
        {
            convertedValue = Enum.Parse(convertType, value);
        }
        else
        {
            GenericObjectToTypeConverter<T>.RaiseCannotConvertException(value);
        }

        return (T?)convertedValue;
    }

    /// <summary>
    /// Raises an exception to indicate that the value could not be converted
    /// </summary>
    /// <param name="value">The value that could not be converted</param>
    private static void RaiseCannotConvertException(object value)
    {
        var valueString = value.ToString();
        var typeName = typeof(T).ToString();

        throw new InvalidCastException($"'{valueString}' cannot be converted to type {typeName}.");
    }
}
