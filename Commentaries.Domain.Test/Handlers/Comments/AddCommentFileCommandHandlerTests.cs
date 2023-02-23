﻿using AutoFixture;
using Commentaries.Data;
using Commentaries.Data.Models;
using Commentaries.Domain.Handlers.Comments.AddCommentFile;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Commentaries.Domain.Test.Handlers.Comments;

public class AddCommentFileCommandHandlerTests
{
    private readonly CommentariesContext _dbContext;
    private readonly Fixture _fixture;
    private readonly IValidator<AddCommentFileCommand> _addCommentFileCommandValidator;

    public AddCommentFileCommandHandlerTests(
        IValidator<AddCommentFileCommand> addCommentFileCommandValidator)
    {
        _dbContext = TestHelper.CreateInMemoryCommentariesContext();
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
