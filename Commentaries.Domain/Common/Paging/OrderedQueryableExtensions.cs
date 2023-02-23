global using static Commentaries.Domain.Common.Paging.OrderedQueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Domain.Common.Paging
{
    internal static class OrderedQueryableExtensions
    {
        public static async Task<Page<T>> PageAsync<T>(this IOrderedQueryable<T> orderedQueryable, IPagingQuery query,
            CancellationToken cancellationToken)
        {
            if (orderedQueryable is null)
            {
                throw new ArgumentNullException(nameof(orderedQueryable));
            }

            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (query.PageNumber < 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(query.PageNumber)} must be greater than or equal to 1");
            }

            var totalCount = await orderedQueryable.CountAsync(cancellationToken: cancellationToken);
            var items = await orderedQueryable.Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return new Page<T>(items, totalCount);
        }
    }
}
