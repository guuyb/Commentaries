using Commentaries.Domain.Models;
using Commentaries.Application.Handlers.Comments.CreateDraftComment;
using Commentaries.Application.Handlers.Comments.PublishComment;
using Commentaries.Infrastructure.SecondaryAdapters.Db;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using ValidationException = Commentaries.Application.Common.Exceptions.ValidationException;

namespace Commentaries.Application.Test.Handlers.Comments;

public class PublishCommentCommandHandlerTests
{
    private readonly CommentariesDbContext _commentDbContext;
    private readonly CreateDraftCommentCommandHandler _createDraftCommentCommandHandler;
    private readonly PublishCommentCommandHandler _publishCommentCommandHandler;

    public PublishCommentCommandHandlerTests(
        IValidator<CreateDraftCommentCommand> createDraftCommentCommandValidator,
        IValidator<PublishCommentCommand> publishCommentCommandValidator)
    {
        _commentDbContext = TestHelper.CreateInMemoryCommentariesDbContext();
        _createDraftCommentCommandHandler = new CreateDraftCommentCommandHandler(
            _commentDbContext,
            createDraftCommentCommandValidator);
        _publishCommentCommandHandler = new PublishCommentCommandHandler(
            _commentDbContext,
            publishCommentCommandValidator);
    }

    [Fact]
    public async Task Should_ThrowException_When_CommentIdIsIncorrect()
    {
        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await _publishCommentCommandHandler.Handle(
                new PublishCommentCommand(CommentId: default),
                default);
        });
    }

    [Fact]
    public async Task Should_PublishComment_When_CommentIdIsProvided()
    {
        var createDraftCommentResult = await _createDraftCommentCommandHandler
            .Handle(new CreateDraftCommentCommand(
                    ObjectId: 1.ToString(),
                    ObjectTypeFullName: typeof(FakeObject).FullName!,
                    AuthorId: 1.ToString(),
                    Content: "SomeContent"),
                default);

        await _publishCommentCommandHandler.Handle(new PublishCommentCommand(
            CommentId: createDraftCommentResult.CommentId), default);

        var publishedComment = await _commentDbContext.Comments
            .FirstAsync(c => c.Id == createDraftCommentResult.CommentId);
        Assert.True(publishedComment.StateId == CommentStateEnum.Published);
        Assert.NotNull(publishedComment.PublishedDate);
    }

    [Fact]
    public async Task Should_ThrowException_When_ContentIsNotProvided()
    {
        var createDraftCommentResult = await _createDraftCommentCommandHandler
            .Handle(new CreateDraftCommentCommand(
                    ObjectId: 1.ToString(),
                    ObjectTypeFullName: typeof(FakeObject).FullName!,
                    AuthorId: 1.ToString(),
                    Content: null),
                default);

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await _publishCommentCommandHandler.Handle(new PublishCommentCommand(
                CommentId: createDraftCommentResult.CommentId), default);
        });
    }

    private class FakeObject
    {
    }
}
