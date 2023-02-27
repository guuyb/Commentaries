using Commentaries.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Commentaries.Application.Ports
{
    public interface ICommentariesDbContext
    {
        DbSet<CommentFile> CommentFiles { get; }
        DbSet<Comment> Comments { get; }
        DbSet<CommentState> CommentStates { get; }
        DbSet<ObjectType> ObjectTypes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void Send(object payload, string targetQueueName, Action<Optionals>? setup = null);
        void Publish(object payload, Action<Optionals>? setup = null);
    }

    public class Optionals
    {
        public string? RoutingKey { get; set; }
        public DateTime? DelayUntil { get; set; }
    }
}