global using static Commentaries.Application.Common.ClassExtensions;
using Commentaries.Application.Common.Exceptions;

namespace Commentaries.Application.Common;

internal static class ClassExtensions
{
    /// <exception cref="NotFoundException"></exception>
    public static void ThrowNotFoundIfNull<T>(this T? entity, object id)
        where T : class
    {
        if (entity == null)
            throw new NotFoundException(typeof(T).GetLocalizedName(), typeof(T).Name, id);
    }
}
