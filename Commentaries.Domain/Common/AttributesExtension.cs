using Commentaries.Data.Attributes;
using System;

namespace Commentaries.Domain.Common;

public static class AttributesExtension
{
    public static string GetLocalizedName<T>()
    {
        return GetLocalizedName(typeof(T));
    }

    public static string GetLocalizedName(this Type type)
    {
        var customAttributes = (LocalizedNameAttribute[])type.GetCustomAttributes(typeof(LocalizedNameAttribute), true);
        if (customAttributes.Length > 0)
        {
            var myAttribute = customAttributes[0];
            return myAttribute.LocalizedName;
        }

        return type.Name;
    }
}
