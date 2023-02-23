using System;

namespace Commentaries.Data.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class LocalizedNameAttribute : Attribute
{
    public string LocalizedName { get; internal set; }

    public LocalizedNameAttribute(string localizedName)
    {
        LocalizedName = localizedName;
    }
}
