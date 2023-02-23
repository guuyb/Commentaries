using AutoFixture;
using Commentaries.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Commentaries.Domain.Test;

internal static class TestHelper
{
    public static CommentariesContext CreateInMemoryCommentariesContext()
    {
        var contextBuilder = new DbContextOptionsBuilder<CommentariesContext>()
            .UseInMemoryDatabase($"CommentariesContext_{Guid.NewGuid()}")
            .EnableSensitiveDataLogging();
        return new CommentariesContext(contextBuilder.Options);
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
