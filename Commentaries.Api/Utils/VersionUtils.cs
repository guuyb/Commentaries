using System;
using System.Reflection;

namespace Commentaries.Api.Utils;

/// <summary>
/// Утилиты для работы с версией
/// </summary>
public static class VersionUtils
{
    /// <summary>
    /// Базовая версия
    /// </summary>
    /// <returns>[major]</returns>
    public static string GetMajorVersion(this string version)
    {
        Version ver = null;

        try
        {
            ver = new Version(version);
        }
        catch (Exception)
        {
        }

        if (ver == null || ver.Minor == -1) return "No version";
        return $"{ver.Major}";
    }

    /// <summary>
    /// Базовая версия
    /// </summary>
    /// <returns>[major].[minor]</returns>
    public static string GetMajorMinorVersion(this string version)
    {
        Version ver = null;

        try
        {
            ver = new Version(version);
        }
        catch (Exception)
        {
        }

        if (ver == null || ver.Minor == -1) return "No version";
        return $"{ver.Major}.{ver.Minor}";
    }

    /// <summary>
    /// Получить версию файла сборки <paramref name = "assembly" />
    /// </summary>
    /// <param name="assembly">Сборка</param>
    /// <returns>Версия файла сборки</returns>
    public static string GetAssemblyFileVersion(Assembly assembly)
    {
        if (assembly == null) return null;
        var attr = assembly.GetAttribute<AssemblyFileVersionAttribute>(false);
        return attr == null ? string.Empty : attr.Version;
    }

    /// <summary>
    /// Возвращает первый атрибут указанного типа либо null
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="provider"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    private static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider provider, bool inherit = true)
        where TAttribute : Attribute
    {
        var attrs = provider?.GetCustomAttributes(typeof(TAttribute), inherit);
        return attrs?.Length > 0 ? (TAttribute)attrs[0] : null;
    }
}
