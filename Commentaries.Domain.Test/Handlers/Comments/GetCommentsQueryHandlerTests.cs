using Commentaries.Data;
using Commentaries.Data.Models;
using Commentaries.Domain.Handlers.Comments.CreateDraftComment;
using Commentaries.Domain.Handlers.Comments.GetComments;
using Commentaries.Domain.Handlers.Comments.PublishNewComment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ValidationException = Commentaries.Domain.Common.Exceptions.ValidationException;

namespace Commentaries.Domain.Test.Handlers.Comments;

public class GetCommentsQueryHandlerTests
{
    private readonly CommentariesContext _dbContext;
    private readonly GetCommentsQueryHandler _getCommentsQueryHandler;
    private readonly CreateDraftCommentCommandHandler _createDraftCommentCommandHandler;
    private readonly PublishNewCommentCommandHandler _publishNewCommentCommandHandler;

    public GetCommentsQueryHandlerTests(IValidator<GetCommentsQuery> getCommentsQueryValidator,
        IValidator<CreateDraftCommentCommand> createDraftCommentCommandValidator,
        IValidator<PublishNewCommentCommand> publishNewCommentCommandValidator)
    {
        _dbContext = TestHelper.CreateInMemoryCommentariesContext();
        _getCommentsQueryHandler = new GetCommentsQueryHandler(
            _dbContext,
            getCommentsQueryValidator);
        _createDraftCommentCommandHandler = new CreateDraftCommentCommandHandler(
            _dbContext,
            createDraftCommentCommandValidator);
        _publishNewCommentCommandHandler = new PublishNewCommentCommandHandler(
            _dbContext,
            publishNewCommentCommandValidator);
    }

    [Fact]
    public async Task Should_GetComments_When_StateNotRequested()
    {
        int expectedDraftCount = 12;
        int expectedPublishedCount = 7;

        int expectedCount = expectedDraftCount + expectedPublishedCount;
        var objectId = Guid.NewGuid().ToString();

        // expected
        await CreateComments(objectId,
            CommentStateEnum.Draft,
            expectedDraftCount,
            typeof(FakeExpectedObject).FullName!);
        await CreateComments(objectId,
            CommentStateEnum.Published,
            expectedPublishedCount,
            typeof(FakeExpectedObject).FullName!);

        // other
        await CreateComments(Guid.NewGuid().ToString(),
            CommentStateEnum.Draft,
            17,
            typeof(FakeUnexpectedObject).FullName!);
        await CreateComments(Guid.NewGuid().ToString(),
            CommentStateEnum.Published,
            23,
            typeof(FakeUnexpectedObject).FullName!);

        var query = new GetCommentsQuery(
            PageNumber: GetCommentsQuery.FIRST_PAGE_NUMBER,
            PageSize: 200,
            Order: GetCommentsQuery.ASCENDING_ORDER,
            Member: nameof(CommentDto.CreatedDate),
            ObjectId: objectId,
            ObjectTypeFullName: typeof(FakeExpectedObject).FullName!,
            StateId: null);
        var result = await _getCommentsQueryHandler.Handle(query, default);

        var comments = result?.Comments;
        Assert.NotNull(comments);
        Assert.NotEmpty(comments);
        Assert.True(comments!.Count == expectedCount);
    }

    [Theory]
    [InlineData(CommentStateEnum.Draft, 7)]
    [InlineData(CommentStateEnum.Published, 11)]
    public async Task Should_GetComments_When_CertainStateRequested(CommentStateEnum expectedStateId, int expectedCount)
    {
        var objectId = Guid.NewGuid().ToString();

        // expected
        await CreateComments(objectId,
            expectedStateId,
            expectedCount,
            typeof(FakeExpectedObject).FullName!);

        // other
        await CreateComments(objectId,
            expectedStateId == CommentStateEnum.Draft // mix expected with another unexpected states
                ? CommentStateEnum.Published
                : CommentStateEnum.Draft,
            13,
            typeof(FakeExpectedObject).FullName!);
        await CreateComments(Guid.NewGuid().ToString(),
            CommentStateEnum.Draft,
            17,
            typeof(FakeUnexpectedObject).FullName!);
        await CreateComments(Guid.NewGuid().ToString(),
            CommentStateEnum.Published,
            23,
            typeof(FakeUnexpectedObject).FullName!);

        var result = await _getCommentsQueryHandler.Handle(
            new GetCommentsQuery(
                PageNumber: GetCommentsQuery.FIRST_PAGE_NUMBER,
                PageSize: 200,
                Order: GetCommentsQuery.ASCENDING_ORDER,
                Member: nameof(CommentDto.CreatedDate),
                StateId: expectedStateId,
                ObjectId: objectId,
                ObjectTypeFullName: typeof(FakeExpectedObject).FullName!),
            default);

        Assert.NotNull(result.Comments);
        Assert.NotEmpty(result.Comments);
        Assert.True(result.Comments.Count == expectedCount);
        Assert.All(result.Comments,
            c => Assert.True(c.StateId == expectedStateId));
    }

    private async Task CreateComments(string objectId,
        CommentStateEnum stateId,
        int commentCount,
        string objectTypeFullName)
    {
        for (int index = 1; index <= commentCount; index++)
        {
            if (stateId == CommentStateEnum.Published)
            {
                await _publishNewCommentCommandHandler.Handle(
                    new PublishNewCommentCommand(
                        Content: $"SomePublishedContent-{index}",
                        PublishedDate: DateTime.UtcNow,
                        ObjectId: objectId,
                        ObjectTypeFullName: objectTypeFullName,
                        AuthorId: 1.ToString()),
                    default);
            }
            else
            {
                await _createDraftCommentCommandHandler.Handle(
                    new CreateDraftCommentCommand(
                        Content: $"SomeDraftContent-{index}",
                        ObjectId: objectId,
                        ObjectTypeFullName: objectTypeFullName,
                        AuthorId: 1.ToString()),
                    default);
            }
        }
    }



    [Theory]
    [MemberData(nameof(GetInvalidQueries))]
    public async Task Should_ThrowException(GetCommentsQuery command)
    {
        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await _getCommentsQueryHandler.Handle(command, default);
        });
    }

    public static IEnumerable<object[]> GetInvalidQueries()
    {
        yield return new object[] {
                new GetCommentsQuery(
                    PageNumber: GetCommentsQuery.FIRST_PAGE_NUMBER,
                    PageSize: 0, // incorrect
                    Order: GetCommentsQuery.ASCENDING_ORDER,
                    Member: nameof(CommentDto.CreatedDate),
                    ObjectId: 1.ToString(),
                    ObjectTypeFullName: typeof(FakeObject).FullName!,
                    StateId: (CommentStateEnum)(-1)) // incorrect
            };

        yield return new object[] {
                new GetCommentsQuery(
                    PageNumber: -1, // incorrect
                    PageSize: 201, // incorrect
                    Order: GetCommentsQuery.ASCENDING_ORDER,
                    Member: nameof(CommentDto.CreatedDate),
                    ObjectId: 1.ToString(),
                    ObjectTypeFullName: typeof(FakeObject).FullName!,
                    StateId: (CommentStateEnum) 999) // incorrect
            };
    }

    private class FakeExpectedObject { }
    private class FakeUnexpectedObject { }

    private class FakeObject { }
}
