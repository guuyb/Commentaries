using Commentaries.Data;
using Commentaries.Data.Models;
using Commentaries.Domain.Handlers.Comments.CreateDraftComment;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ValidationException = Commentaries.Domain.Common.Exceptions.ValidationException;

namespace Commentaries.Domain.Test.Handlers.Comments;

public class CreateDraftCommentCommandHandlerTests
{
    private readonly CommentariesContext _dbContext;
    private readonly CreateDraftCommentCommandHandler _handler;

    public CreateDraftCommentCommandHandlerTests(
        IValidator<CreateDraftCommentCommand> commandValidator)
    {
        _dbContext = TestHelper.CreateInMemoryCommentariesContext();
        _handler = new CreateDraftCommentCommandHandler(_dbContext, commandValidator);

    }

    [Theory]
    [MemberData(nameof(GetCommands))]
    public async Task Should_CreateDraftComment(CreateDraftCommentCommand command)
    {
        var result = await _handler.Handle(command, default);
        Assert.NotNull(result);
        Assert.True(result.CommentId != default);

        var createdComment = await _dbContext.Comments
            .FirstAsync(c => c.Id == result.CommentId);
        Assert.True(createdComment!.StateId == CommentStateEnum.Draft);
    }

    public static IEnumerable<object[]> GetCommands()
    {
        // Content is not provided
        yield return new object[] { new CreateDraftCommentCommand(
                    Content: null,
                    ObjectId: Guid.NewGuid().ToString(),
                    ObjectTypeFullName: typeof(FakeObject).FullName!,
                    AuthorId: Guid.NewGuid().ToString()
                )};

        // Content is provided
        yield return new object[] { new CreateDraftCommentCommand(
                    Content: "SomeContent",
                    ObjectId: Guid.NewGuid().ToString(),
                    ObjectTypeFullName: typeof(FakeObject).FullName!,
                    AuthorId: Guid.NewGuid().ToString()
                )};
    }

    [Fact]
    public async Task Should_ThrowException()
    {
        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await _handler.Handle(new CreateDraftCommentCommand(string.Empty, null, string.Empty, string.Empty), default);
        });
    }

    private class FakeObject
    {
    }
}
