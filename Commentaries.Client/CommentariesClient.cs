using Commentaries.Client.Models.AddCommentFile;
using Commentaries.Client.Models.CreateDraftComment;
using Commentaries.Client.Models.GetComment;
using Commentaries.Client.Models.GetCommentFile;
using Commentaries.Client.Models.GetComments;
using Commentaries.Client.Models.GetObjectIdToCommentCountMap;
using Commentaries.Client.Models.PublishComment;
using Flurl.Http;
using Flurl.Http.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Client;

public interface ICommentariesClient
{
    Task<AddCommentFileResultDto> AddCommentFileAsync(Guid commentId, AddCommentFileBodyDto body, CancellationToken cancellation);
    Task<CreateDraftCommentResultDto> CreateDraftCommentAsync(CreateDraftCommentBodyDto body, CancellationToken cancellation);
    Task EditCommentContentAsync(Guid commentId, string content, CancellationToken cancellation);
    Task<GetCommentResultDto> GetCommentAsync(Guid commentId, CancellationToken cancellation);
    Task<GetCommentFileResultDto> GetCommentFileAsync(Guid commentId, Guid commentFileId, CancellationToken cancellation);
    Task<GetCommentsResultDto> GetCommentsAsync(GetCommentsQueryDto query, CancellationToken cancellation);
    Task<GetObjectIdToCommentCountMapResultDto> GetObjectIdToCommentCountMapAsync(string objectTypeFullName, string[] objectIds, CancellationToken cancellation);
    Task PublishCommentAsync(Guid commentId, CancellationToken cancellation);
    Task<PublishNewCommentResultDto> PublishNewCommentAsync(PublishNewCommentBodyDto body, CancellationToken cancellation);
    Task RemoveCommentFileAsync(Guid commentId, Guid commentFileId, CancellationToken cancellation);
}

public class CommentariesClient : ICommentariesClient
{
    private const string BASE_PATH = "api/comments";
    private readonly IFlurlClient _client;

    public CommentariesClient(
        IFlurlClientFactory clientFactory,
        CommentariesApiConfig config)
    {
        _client = clientFactory.Get(config.BaseUrl);
    }

    internal CommentariesClient(IFlurlClient client)
    {
        _client = client;
    }

    public Task<GetCommentsResultDto> GetCommentsAsync(
        GetCommentsQueryDto query,
        CancellationToken cancellation)
    {
        return _client
            .Request(BASE_PATH)
            .SetQueryParams(query)
            .GetJsonAsync<GetCommentsResultDto>(cancellation);
    }

    public Task<GetCommentResultDto> GetCommentAsync(
        Guid commentId,
        CancellationToken cancellation)
    {
        return _client
            .Request($"{BASE_PATH}/{commentId}")
            .GetJsonAsync<GetCommentResultDto>(cancellation);
    }

    public Task<CreateDraftCommentResultDto> CreateDraftCommentAsync(
        CreateDraftCommentBodyDto body,
        CancellationToken cancellation)
    {
        return _client
            .Request(BASE_PATH)
            .PostJsonAsync(body, cancellation)
            .ReceiveJson<CreateDraftCommentResultDto>();
    }

    public Task PublishCommentAsync(
        Guid commentId,
        CancellationToken cancellation)
    {
        return _client
            .Request($"{BASE_PATH}/{commentId}/publish")
            .PostAsync(cancellationToken: cancellation);
    }

    public Task EditCommentContentAsync(
        Guid commentId,
        string content,
        CancellationToken cancellation)
    {
        return _client
            .Request($"{BASE_PATH}/{commentId}")
            .PutJsonAsync(new { content }, cancellation);
    }

    public Task<PublishNewCommentResultDto> PublishNewCommentAsync(
        PublishNewCommentBodyDto body,
        CancellationToken cancellation)
    {
        return _client
            .Request($"{BASE_PATH}/published")
            .PostJsonAsync(body, cancellation)
            .ReceiveJson<PublishNewCommentResultDto>();
    }

    public Task<GetObjectIdToCommentCountMapResultDto> GetObjectIdToCommentCountMapAsync(
        string objectTypeFullName,
        string[] objectIds,
        CancellationToken cancellation)
    {
        return _client
            .Request($"{BASE_PATH}/object-type/{objectTypeFullName}/counts")
            .SetQueryParams(objectIds)
            .GetJsonAsync<GetObjectIdToCommentCountMapResultDto>(cancellation);
    }

    public Task<AddCommentFileResultDto> AddCommentFileAsync(
        Guid commentId,
        AddCommentFileBodyDto body,
        CancellationToken cancellation)
    {
        return _client
            .Request($"{BASE_PATH}/{commentId}/files")
            .PostMultipartAsync(
                    bc => bc.AddFile("file", body.Content, body.FileName),
                    cancellation)
            .ReceiveJson<AddCommentFileResultDto>();
    }

    public Task<GetCommentFileResultDto> GetCommentFileAsync(
        Guid commentId,
        Guid commentFileId,
        CancellationToken cancellation)
    {
        return _client
            .Request($"{BASE_PATH}/{commentId}/files/{commentFileId}")
            .GetJsonAsync<GetCommentFileResultDto>(cancellation);
    }

    public Task RemoveCommentFileAsync(
        Guid commentId,
        Guid commentFileId,
        CancellationToken cancellation)
    {
        return _client
            .Request($"{BASE_PATH}/{commentId}/files/{commentFileId}")
            .DeleteAsync(cancellation);
    }
}
