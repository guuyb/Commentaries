using Commentaries.Domain.Models;
using Commentaries.Application.Ports;
using Microsoft.EntityFrameworkCore;

namespace Commentaries.Infrastructure.SecondaryAdapters.Db;

public class CommentariesDbContext : DbContext, ICommentariesDbContext
{
    private readonly DbConfigurationsOptions _dbConfigurationsOptions;

    public CommentariesDbContext(
        DbContextOptions<CommentariesDbContext> options,
        DbConfigurationsOptions dbConfigurationsOptions) : base(options)
    {
        _dbConfigurationsOptions = dbConfigurationsOptions;
    }

    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentState> CommentStates => Set<CommentState>();
    public DbSet<ObjectType> ObjectTypes => Set<ObjectType>();
    public DbSet<CommentFile> CommentFiles => Set<CommentFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(_dbConfigurationsOptions.Assembly);
    }
}
