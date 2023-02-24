using AutoFixture;
using Commentaries.Domain.Models;
using Commentaries.Application.Handlers.Comments.AddCommentFile;
using Commentaries.Infrastructure.SecondaryAdapters.Db;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Commentaries.Application.Test.Handlers.Comments;

public class AddCommentFileCommandHandlerTests
{
    private readonly CommentariesDbContext _dbContext;
    private readonly Fixture _fixture;
    private readonly IValidator<AddCommentFileCommand> _addCommentFileCommandValidator;

    public AddCommentFileCommandHandlerTests(
        IValidator<AddCommentFileCommand> addCommentFileCommandValidator)
    {
        _dbContext = TestHelper.CreateInMemoryCommentariesDbContext();
        _fixture = TestHelper.CreateFixtureWithOmitOnRecursionBehavior();
        _addCommentFileCommandValidator = addCommentFileCommandValidator;
    }

    [Theory]
    [InlineData(CommentStateEnum.Draft)]
    [InlineData(CommentStateEnum.Published)]
    public async Task Should_add_comment_file_when_comment_is_in_a_certain_state(CommentStateEnum state)
    {
        // arrange
        var comment = _fixture.Build<Comment>()
            .With(c => c.StateId, state)
            .With(c => c.State, () => null)
            .Create();
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();

        using var fileData = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        var expectedFileName = "test.txt";

        // act
        var command = new AddCommentFileCommand(
            CommentId: comment.Id,
            FileName: expectedFileName,
            Data: fileData);
        await new AddCommentFileCommandHandler(
            _dbContext,
            _addCommentFileCommandValidator).Handle(command, default);

        // assert
        var addedCommentFile = await _dbContext.CommentFiles
            .FirstOrDefaultAsync(f => f.FileName == expectedFileName
                && f.CommentId == comment.Id);
        Assert.NotNull(addedCommentFile);
    }
}
