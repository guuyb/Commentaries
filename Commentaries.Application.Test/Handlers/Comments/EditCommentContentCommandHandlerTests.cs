using Commentaries.Application.Handlers.Comments.CreateDraftComment;
using Commentaries.Application.Handlers.Comments.EditCommentContent;
using Commentaries.Infrastructure.SecondaryAdapters.Db;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using ValidationException = Commentaries.Application.Common.Exceptions.ValidationException;

namespace Commentaries.Application.Test.Handlers.Comments;

public class EditCommentContentCommandHandlerTests
{
    private readonly CommentariesDbContext _dbContext;
    private readonly CreateDraftCommentCommandHandler _createDraftCommentCommandHandler;
    private readonly EditCommentContentCommandHandler _editCommentContentCommandHandler;

    public EditCommentContentCommandHandlerTests(
        IValidator<CreateDraftCommentCommand> createDraftCommentCommandValidator,
        IValidator<EditCommentContentCommand> editCommentContentCommandValidator)
    {
        _dbContext = TestHelper.CreateInMemoryCommentariesDbContext();
        _createDraftCommentCommandHandler = new CreateDraftCommentCommandHandler(
            _dbContext,
            createDraftCommentCommandValidator);
        _editCommentContentCommandHandler = new EditCommentContentCommandHandler(
            _dbContext,
            editCommentContentCommandValidator);
    }

    [Fact]
    public async Task Should_ThrowException_When_CommandInvalid()
    {
        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var command = new EditCommentContentCommand(
                CommentId: default, // incorrect
                Content: null
            );
            await _editCommentContentCommandHandler.Handle(command, default);
        });
    }

    [Fact]
    public async Task Should_EditCommentContent_When_CommentIsDraft()
    {
        var createCommand = new CreateDraftCommentCommand(
            ObjectId: 1.ToString(),
            ObjectTypeFullName: typeof(FakeObject).FullName!,
            AuthorId: 1.ToString(),
            Content: "SomeContent"
        );
        var createDraftCommentResult = await _createDraftCommentCommandHandler
            .Handle(createCommand, default);
        var expectedContent = "SomeAnotherContent";

        var editCommand = new EditCommentContentCommand(
            CommentId: createDraftCommentResult.CommentId,
            Content: expectedContent);
        await _editCommentContentCommandHandler.Handle(editCommand, default);

        var createdComment = await _dbContext.Comments
            .FirstAsync(c => c.Id == createDraftCommentResult.CommentId);
        Assert.True(createdComment!.Content == expectedContent);
    }

    private class FakeObject
    {
    }
}
