global using static Commentaries.Data.Extensions.PropertyBuilderExtensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Commentaries.Data.Extensions;

internal static class PropertyBuilderExtensions
{
    /// <summary>
    /// Преобразует дату к UTC, если она локальная (Kind=Unspecified воспринимается как локальная дата)
    /// </summary>
    public static PropertyBuilder<DateTime> HasUtcDateTimeConversion(this PropertyBuilder<DateTime> propertyBuilder)
        => propertyBuilder.HasConversion(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

    /// <summary>
    /// Преобразует дату к UTC, если она локальная (Kind=Unspecified воспринимается как локальная дата)
    /// </summary>
    public static PropertyBuilder<DateTime?> HasUtcDateTimeConversion(this PropertyBuilder<DateTime?> propertyBuilder)
        => propertyBuilder.HasConversion(
            (DateTime? v) => v.HasValue
                ? v.Value.ToUniversalTime()
                : null,
            (DateTime? v) => v.HasValue
                ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                : null);

}
