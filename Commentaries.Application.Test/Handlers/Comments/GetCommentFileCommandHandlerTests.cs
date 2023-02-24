using AutoFixture;
using Commentaries.Domain.Models;
using Commentaries.Application.Handlers.Comments.GetCommentFile;
using Commentaries.Infrastructure.SecondaryAdapters.Db;
using FluentValidation;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Commentaries.Application.Test.Handlers.Comments;

public class GetCommentFileCommandHandlerTests
{
    private readonly CommentariesDbContext _dbContext;
    private readonly Fixture _fixture;
    private readonly IValidator<GetCommentFileQuery> _getCommentFileQueryValidator;

    public GetCommentFileCommandHandlerTests(
        IValidator<GetCommentFileQuery> getCommentFileQueryValidator)
    {
        _dbContext = TestHelper.CreateInMemoryCommentariesDbContext();
        _fixture = TestHelper.CreateFixtureWithOmitOnRecursionBehavior();
        _getCommentFileQueryValidator = getCommentFileQueryValidator;
    }

    [Theory]
    [InlineData(CommentStateEnum.Draft)]
    [InlineData(CommentStateEnum.Published)]
    public async Task Should_get_comment_file_when_comment_is_in_a_certain_state(CommentStateEnum state)
    {
        // arrange
        var commentFile = _fixture.Build<CommentFile>()
            .With(c => c.IsDeleted, false)
            .Create();
        var comment = _fixture.Build<Comment>()
            .With(c => c.StateId, state)
            .With(c => c.State, () => null)
            .With(c => c.Files, new CommentFile[] { commentFile })
            .Create();
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();

        var expectedFileName = commentFile.FileName;

        // act
        var query = new GetCommentFileQuery(
            CommentId: comment.Id,
            CommentFileId: commentFile.Id);
        var getCommentFileResult = await new GetCommentFileQueryHandler(
            _dbContext,
            _getCommentFileQueryValidator).Handle(query, default);

        // assert
        Assert.Equal(expectedFileName, getCommentFileResult.File.FileName);
    }
}
