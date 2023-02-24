namespace Commentaries.Application.Common.Paging
{
    public interface IPagingQuery
    {
        public int PageNumber { get; }
        public int PageSize { get; }
    }
}
