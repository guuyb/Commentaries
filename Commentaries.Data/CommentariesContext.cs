using Microsoft.EntityFrameworkCore;
using Commentaries.Data.Models;
using System.Reflection;

namespace Commentaries.Data;

public class CommentariesContext : DbContext
{
    public CommentariesContext(DbContextOptions<CommentariesContext> options)
        : base(options)
    {
    }

    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentState> CommentStates => Set<CommentState>();
    public DbSet<ObjectType> ObjectTypes => Set<ObjectType>();
    public DbSet<CommentFile> CommentFiles => Set<CommentFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
