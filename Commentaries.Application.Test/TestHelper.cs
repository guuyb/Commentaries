using AutoFixture;
using Commentaries.Infrastructure.SecondaryAdapters.Db;
using Commentaries.Infrastructure.SecondaryAdapters.Db.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Commentaries.Application.Test;

internal static class TestHelper
{
    public static CommentariesDbContext CreateInMemoryCommentariesDbContext()
    {
        var contextBuilder = new DbContextOptionsBuilder<CommentariesDbContext>()
            .UseInMemoryDatabase($"CommentariesDbContext_{Guid.NewGuid()}")
            .EnableSensitiveDataLogging();
        return new CommentariesDbContext(contextBuilder.Options,
            new DbConfigurationsOptions(typeof(CommentConfiguration).Assembly));
    }

    public static Fixture CreateFixtureWithOmitOnRecursionBehavior()
    {
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}
