global using static Commentaries.Domain.Common.Paging.QueryableExtensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Commentaries.Domain.Common.Paging;

#nullable disable
internal static class QueryableExtensions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderByProperty, int sortOrder)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (orderByProperty is null)
        {
            throw new ArgumentNullException(nameof(orderByProperty));
        }

        var type = typeof(T);
        var parameter = Expression.Parameter(type, "x");

        PropertyInfo property;
        Expression propertyAccess;
        if (orderByProperty.Contains('.'))
        {
            // support to be sorted on child fields.
            var childProperties = orderByProperty.Split('.');
            property = typeof(T).GetProperty(childProperties[0]);
            propertyAccess = Expression.MakeMemberAccess(parameter, property);

            for (int i = 1; i < childProperties.Length; i++)
            {
                var t = property.PropertyType;
                if (!t.IsGenericType)
                {
                    property = t.GetProperty(childProperties[i]);
                }
                else
                {
                    property = t.GetGenericArguments().First().GetProperty(childProperties[i]);
                }

                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            }
        }
        else
        {
            property = type.GetProperty(orderByProperty);
            propertyAccess = Expression.MakeMemberAccess(parameter, property);
        }

        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var sortOrderMethod = sortOrder < 0 ? "OrderByDescending" : "OrderBy";

        var resultExpression = Expression.Call(typeof(Queryable), sortOrderMethod,
            new Type[] { type, property.PropertyType },
            source.Expression, Expression.Quote(orderByExpression));

        return source.Provider.CreateQuery<T>(resultExpression) as IOrderedQueryable<T>;
    }
}
#nullable disable

