using Commentaries.Domain.Models;
using Commentaries.Application.Ports;
using Microsoft.EntityFrameworkCore;
using Guuyb.OutboxMessaging.Data;
using Commentaries.Infrastructure.SecondaryAdapters.Db.Models;
using static Guuyb.OutboxMessaging.Data.OutboxMessageContextExtentions;
using Optionals = Commentaries.Application.Ports.Optionals;

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

    #region OutboxMessaging
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<OutboxMessageState> OutboxMessageStates => Set<OutboxMessageState>();
    #endregion

    public void Send(object payload,
        string targetQueueName,
        Action<Optionals>? setup)
    {
        var optionals = new Optionals();
        setup?.Invoke(optionals);

        OutboxMessages.Send(payload, targetQueueName, o => SetupOptionals(o, setup));
    }

    public void Publish(object payload, Action<Optionals>? setup)
    {
        OutboxMessages.Publish(payload, o => SetupOptionals(o, setup));
    }

    private void SetupOptionals(OutboxMessageContextExtentions.Optionals output, Action<Optionals>? setupInput)
    {
        var input = new Optionals();
        setupInput?.Invoke(input);
        output.RoutingKey = input.RoutingKey;
        output.DelayUntil = input.DelayUntil;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(_dbConfigurationsOptions.Assembly);
        modelBuilder.ApplyDbSetOutboxMessagingConfiguration<OutboxMessage, OutboxMessageState>(
            nameof(OutboxMessage), nameof(OutboxMessageState));
    }
}
