using Commentaries.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace Commentaries.Application.Ports
{
    public interface ICommentariesDbContext
    {
        DbSet<CommentFile> CommentFiles { get; }
        DbSet<Comment> Comments { get; }
        DbSet<CommentState> CommentStates { get; }
        DbSet<ObjectType> ObjectTypes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}