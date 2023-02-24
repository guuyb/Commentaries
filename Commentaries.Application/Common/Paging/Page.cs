using System.Collections.Generic;

namespace Commentaries.Application.Common.Paging
{
    public class Page<T>
    {
        public long TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }

        public Page(IEnumerable<T> items, long totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}
