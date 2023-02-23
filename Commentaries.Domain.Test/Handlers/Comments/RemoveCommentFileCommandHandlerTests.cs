using AutoFixture;
using Commentaries.Data;
using Commentaries.Data.Models;
using Commentaries.Domain.Handlers.Comments.AddCommentFile;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Commentaries.Domain.Test.Handlers.Comments;

public class RemoveCommentFileCommandHandlerTests
{
    private readonly CommentariesContext _dbContext;
    private readonly Fixture _fixture;
    private readonly IValidator<RemoveCommentFileCommand> _removeCommentFileCommandValidator;

    public RemoveCommentFileCommandHandlerTests(
        IValidator<RemoveCommentFileCommand> removeCommentFileCommandValidator)
    {
        _dbContext = TestHelper.CreateInMemoryCommentariesContext();
        _fixture = TestHelper.CreateFixtureWithOmitOnRecursionBehavior();
        _removeCommentFileCommandValidator = removeCommentFileCommandValidator;
    }

    [Theory]
    [InlineData(CommentStateEnum.Draft)]
    [InlineData(CommentStateEnum.Published)]
    public async Task Should_remove_comment_file_when_comment_is_in_a_certain_state(CommentStateEnum state)
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

        // act
        await new RemoveCommentFileCommandHandler(
            _dbContext,
            _removeCommentFileCommandValidator).Handle(new (comment.Id, commentFile.Id), default);

        // assert
        var removedCommentFile = await _dbContext.CommentFiles.FirstOrDefaultAsync(f => f.Id == commentFile.Id);
        Assert.NotNull(removedCommentFile);
        Assert.True(removedCommentFile!.IsDeleted);
    }
}
