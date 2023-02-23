﻿using Commentaries.Data;
using Commentaries.Data.Models;
using Commentaries.Domain.Handlers.Comments.PublishNewComment;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using ValidationException = Commentaries.Domain.Common.Exceptions.ValidationException;

namespace Commentaries.Domain.Test.Handlers.Comments;

public class PublishNewCommentCommandHandlerTests
{
    private readonly CommentariesContext _dbContext;
    private readonly PublishNewCommentCommandHandler _publishNewCommentCommandHandler;

    public PublishNewCommentCommandHandlerTests(
        IValidator<PublishNewCommentCommand> publishNewCommentCommandValidator)
    {
        _dbContext = TestHelper.CreateInMemoryCommentariesContext();
        _publishNewCommentCommandHandler = new PublishNewCommentCommandHandler(
            _dbContext,
            publishNewCommentCommandValidator);
    }

    [Fact]
    public async Task Should_PublishNewComment_When_ContentIsProvided()
    {
        var command = new PublishNewCommentCommand(
            Content: "SomeContent",
            PublishedDate: DateTime.UtcNow,
            ObjectId: Guid.NewGuid().ToString(),
            ObjectTypeFullName: typeof(FakeObject).FullName!,
            AuthorId: 1.ToString());

        var result = await _publishNewCommentCommandHandler.Handle(command, default);
        Assert.NotNull(result);
        Assert.True(result.CommentId != default);

        var publishedComment = await _dbContext.Comments.FirstAsync(c => c.Id == result.CommentId);
        Assert.True(publishedComment.StateId == CommentStateEnum.Published);
        Assert.NotNull(publishedComment.PublishedDate);
    }

    [Fact]
    public async Task Should_ThrowException_When_ContentIsNotProvided()
    {
        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var command = new PublishNewCommentCommand(
                Content: string.Empty,
                PublishedDate: DateTime.UtcNow,
                AuthorId: 1.ToString(),
                ObjectId: 1.ToString(),
                ObjectTypeFullName: typeof(FakeObject).FullName!);
            await _publishNewCommentCommandHandler.Handle(command, default);
        });
    }

    private class FakeObject
    {
    }
}
