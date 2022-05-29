namespace Nettle;

using Nettle.Common.Serialization.Grid;

public static class TypeExtensions
{
    /// <summary>
    /// Determines if the type can be converted to the type specified from the given type value
    /// </summary>
    /// <param name="fromType">The current type</param>
    /// <param name="toType">The new type</param>
    /// <param name="fromObject">The current type value</param>
    /// <returns>True, if the type can be converted; otherwise false</returns>
    public static bool CanConvert(this Type fromType, Type toType, object fromObject)
    {
        Validate.IsNotNull(fromType);
        Validate.IsNotNull(toType);

        if (fromObject == null)
        {
            return toType.IsNullable();
        }
        else
        {
            var fromObjectType = fromObject.GetType();

            if (fromObjectType == toType)
            {
                return true;
            }
            else if (toType.IsAssignableFrom(fromObjectType) || fromObjectType.IsAssignableFrom(toType))
            {
                return true;
            }
            else
            {
                if (false == fromObjectType.ImplementsInterface(typeof(IConvertible)))
                {
                    return false;
                }

                var converterType = typeof(TypeConverterChecker<,>).MakeGenericType(fromType, toType);
                var instance = Activator.CreateInstance(converterType, fromObject);

                var canConvertProperty = converterType.GetProperty("CanConvert");

                if (instance != null && canConvertProperty != null)
                {
                    return (bool?)canConvertProperty.GetGetMethod()?.Invoke(instance, null) ?? false;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    /// <summary>
    /// Determines if the type has a property with the name specified
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <param name="propertyName">The name of the property to look for</param>
    /// <returns>True, if the property exists; otherwise false</returns>
    public static bool HasProperty(this Type type, string propertyName)
    {
        return type.GetProperty(propertyName) != null;
    }

    /// <summary>
    /// Determines if a type is numeric. Nullable numeric types are considered numeric.
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <param name="acceptNullables">If true nullable numeric types are also accepted</param>
    /// <returns>True, if the type is numeric; otherwise false</returns>
    public static bool IsNumeric(this Type type, bool acceptNullables = true)
    {
        if (type == null)
        {
            return false;
        }

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Single:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return true;

            case TypeCode.Object:

                if (acceptNullables && type.IsGenericType)
                {
                    var isNullable = type.GetGenericTypeDefinition() == typeof(Nullable<>);

                    if (isNullable)
                    {
                        var underlyingType = Nullable.GetUnderlyingType(type);

                        return underlyingType != null && IsNumeric(underlyingType);
                    }
                }

                return false;
        }

        return false;
    }

    /// <summary>
    /// Determines if the type specified is a nullable type
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <returns>True, if the type is nullable; otherwise false</returns>
    public static bool IsNullable(this Type type)
    {
        return false == type.IsValueType || Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    /// Determines if the type specified is an enumerable type
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <param name="acceptStrings">If true, string types will resolve to true</param>
    /// <returns>True, if the type is enumerable; otherwise false</returns>
    /// <remarks>
    /// All enumerable types are allowed, except for string
    /// </remarks>
    public static bool IsEnumerable(this Type type, bool acceptStrings = true)
    {
        if (false == acceptStrings && type == typeof(string))
        {
            return false;
        }
        else
        {
            return type.GetInterfaces().Contains(typeof(IEnumerable));
        }
    }

    /// <summary>
    /// Determines if the type specified if a data grid
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns>True, if the type is a data grid; otherwise false</returns>
    public static bool IsDataGrid(this Type type)
    {
        return type.GetInterfaces().Contains(typeof(IDataGrid));
    }

    /// <summary>
    /// Determines if the type implements a specific interface
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <param name="interfaceType">The interface type</param>
    /// <returns>True, if the type implements the interface; otherwise false</returns>
    public static bool ImplementsInterface(this Type type, Type interfaceType)
    {
        return type.GetInterfaces().Contains(interfaceType);
    }
}
