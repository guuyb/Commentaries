using Commentaries.Application.Handlers.Comments.CreateDraftComment;
using Commentaries.Application.Handlers.Comments.GetObjectIdToCommentCountMap;
using Commentaries.Application.Handlers.Comments.PublishNewComment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Commentaries.Application.Test.Handlers.Comments;

public class GetObjectIdToCommentCountMapQueryHandlerTests
{
    private readonly GetObjectIdToCommentCountMapQueryHandler _getObjectIdToCommentCountMapQueryHandler;
    private readonly CreateDraftCommentCommandHandler _createDraftCommentCommandHandler;
    private readonly PublishNewCommentCommandHandler _publishNewCommentCommandHandler;

    public GetObjectIdToCommentCountMapQueryHandlerTests(
        IValidator<GetObjectIdToCommentCountMapQuery> getObjectIdToCommentCountMapQueryValidator,
        IValidator<CreateDraftCommentCommand> createDraftCommentCommandValidator,
        IValidator<PublishNewCommentCommand> publishNewCommentCommandValidator)
    {
        var dbContext = TestHelper.CreateInMemoryCommentariesDbContext();
        _getObjectIdToCommentCountMapQueryHandler = new GetObjectIdToCommentCountMapQueryHandler(
            dbContext,
            getObjectIdToCommentCountMapQueryValidator);
        _createDraftCommentCommandHandler = new CreateDraftCommentCommandHandler(
            dbContext,
            createDraftCommentCommandValidator);
        _publishNewCommentCommandHandler = new PublishNewCommentCommandHandler(
            dbContext,
            publishNewCommentCommandValidator);
    }

    [Fact]
    public async Task Should_GetObjectIdToCommentCountMap()
    {
        int expectedCount = 7;
        var objectId = Guid.NewGuid().ToString();

        // expected
        for (var index = 0; index < expectedCount; index++)
        {
            await _publishNewCommentCommandHandler.Handle(
                new PublishNewCommentCommand(
                    Content: "Test content",
                    PublishedDate: DateTime.UtcNow,
                    ObjectId: objectId,
                    ObjectTypeFullName: typeof(FakeExpectedObject).FullName!,
                    AuthorId: 1.ToString()
                ), default);
        }

        // unexpected
        for (var index = 0; index < 13; index++)
        {
            await _createDraftCommentCommandHandler.Handle(
                new CreateDraftCommentCommand(
                    Content: "Test content",
                    ObjectId: objectId,
                    ObjectTypeFullName: typeof(FakeExpectedObject).FullName!,
                    AuthorId: 1.ToString()),
                default);
        }
        for (var index = 0; index < 19; index++)
        {
            await _publishNewCommentCommandHandler.Handle(
                new PublishNewCommentCommand(
                    Content: "Test content",
                    PublishedDate: DateTime.UtcNow,
                    ObjectId: objectId,
                    ObjectTypeFullName: typeof(FakeUnexpectedObject).FullName!,
                    AuthorId: 1.ToString()
                ), default);
        }

        var result = await _getObjectIdToCommentCountMapQueryHandler.Handle(
            new GetObjectIdToCommentCountMapQuery(
                ObjectIds: new[] { objectId },
                ObjectTypeFullName: typeof(FakeExpectedObject).FullName!
            ), default);

        var objectIdToCommentCountMap = result.ObjectIdToCommentCountMap;
        Assert.NotNull(objectIdToCommentCountMap);
        Assert.Contains(objectId, (IDictionary<string, int>)objectIdToCommentCountMap!);
        Assert.True(objectIdToCommentCountMap[objectId] == expectedCount);
    }

    private class FakeExpectedObject { }

    private class FakeUnexpectedObject { }
}
