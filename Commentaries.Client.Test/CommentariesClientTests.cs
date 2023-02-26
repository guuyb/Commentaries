using AutoFixture;
using Commentaries.Api;
using Commentaries.Application.Handlers.Comments.CreateDraftComment;
using Commentaries.Application.Handlers.Comments.GetComments;
using Commentaries.Application.Handlers.Comments.PublishComment;
using Commentaries.Client.Models.CreateDraftComment;
using Commentaries.Client.Models.GetComments;
using Flurl.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using CreateDraftCommentResultDto = Commentaries.Application.Handlers.Comments.CreateDraftComment.CreateDraftCommentResultDto;
using GetCommentsResultDto = Commentaries.Application.Handlers.Comments.GetComments.GetCommentsResultDto;

namespace Commentaries.Client.Test;

public class CommentariesClientTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;
    private readonly Fixture _fixture;

    public CommentariesClientTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Should_get_comments()
    {
        var queryDto = new GetCommentsQueryDto(
            PageNumber: 1,
            PageSize: 200,
            Order: 1,
            Member: "Id");

        var expectedQuery = new GetCommentsQuery(
            PageNumber: queryDto.PageNumber,
            PageSize: queryDto.PageSize,
            Order: queryDto.Order,
            Member: queryDto.Member);

        var expectedResult = _fixture.Create<GetCommentsResultDto>();

        var stubHandler = Mock.Of<IRequestHandler<GetCommentsQuery, GetCommentsResultDto>>(h =>
            h.Handle(expectedQuery, It.IsAny<CancellationToken>()) == Task.FromResult(expectedResult));

        var resultDto = await CreateClient(services =>
        {
            ReplaceHandler(services, stubHandler);
        }).GetCommentsAsync(queryDto, default);

        Assert.NotNull(resultDto);
        Assert.Equal(expectedResult.Comments.Count, resultDto.Comments.Count);
        Assert.Subset(
            expectedResult.Comments.Select(c => c.Id).ToHashSet(),
            resultDto.Comments.Select(c => c.Id).ToHashSet());
        Assert.Equal(expectedResult.TotalCount, resultDto.TotalCount);
    }

    [Fact]
    public async Task Should_create_draft_comment()
    {
        var bodyDto = _fixture.Create<CreateDraftCommentBodyDto>();

        var expectedCommand = new CreateDraftCommentCommand(
            AuthorId: bodyDto.AuthorId,
            Content: bodyDto.Content,
            ObjectId: bodyDto.ObjectId,
            ObjectTypeFullName: bodyDto.ObjectTypeFullName);

        var expectedResult = _fixture.Create<CreateDraftCommentResultDto>();

        var stubHandler = Mock.Of<IRequestHandler<CreateDraftCommentCommand, CreateDraftCommentResultDto>>(h =>
            h.Handle(expectedCommand, It.IsAny<CancellationToken>()) == Task.FromResult(expectedResult));

        var resultDto = await CreateClient(services =>
        {
            ReplaceHandler(services, stubHandler);
        }).CreateDraftCommentAsync(bodyDto, default);

        Assert.Equal(expectedResult.CommentId, resultDto.CommentId);
    }

    [Fact]
    public async Task Should_publish_comment()
    {
        var commentId = _fixture.Create<Guid>();

        var expectedCommand = new PublishCommentCommand(commentId);

        var expectedResult = Task.FromResult(Unit.Value);

        var mockHandler = Mock.Of<IRequestHandler<PublishCommentCommand, Unit>>(h =>
            h.Handle(expectedCommand, It.IsAny<CancellationToken>()) == expectedResult);

        await CreateClient(services =>
        {
            ReplaceHandler(services, mockHandler);
        }).PublishCommentAsync(commentId, default);

        Mock.Get(mockHandler)
            .Verify(h => h.Handle(expectedCommand, It.IsAny<CancellationToken>()));

        Assert.True(true);
    }

    private CommentariesClient CreateClient(Action<IServiceCollection>? servicesConfiguration = null)
    {
        var httpClient = _factory
            .WithWebHostBuilder(builder =>
            {
                if (servicesConfiguration != null)
                {
                    builder.ConfigureTestServices(servicesConfiguration);
                }
            })
            .CreateClient();

        return new CommentariesClient(new FlurlClient(httpClient));
    }

    private void ReplaceHandler<TRequest, TResponse>(IServiceCollection services,
        IRequestHandler<TRequest, TResponse> replacement)
        where TRequest : IRequest<TResponse>
    {
        var handler = services.Single(d => d.ServiceType ==
                typeof(IRequestHandler<TRequest, TResponse>));
        services.Remove(handler);
        services.AddTransient(sp => replacement);
    }
}
