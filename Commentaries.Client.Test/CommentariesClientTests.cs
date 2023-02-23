using Flurl.Http;
using Flurl.Http.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Commentaries.Client.Test;

public class CommentariesClientTests
{
    private const int EXPECTED_I_AM_TEAPOT = 418;
    private const string OBJECT_TYPE_FULL_NAME = "test.test";
    private const string OBJECT_ID = "1";
    private const string AUTHOR_ID = "1";

    private readonly ICommentariesClient _client;
    private readonly Guid _commentId = Guid.NewGuid();
    private readonly Guid _commentFileId = Guid.NewGuid();

    public CommentariesClientTests()
    {
        _client = new CommentariesClient(
            new PerBaseUrlFlurlClientFactory(),
            new CommentariesApiConfig { 
                BaseUrl = "http://localhost:53450", 
                IsClientTest = true 
            });
    }

    [Fact]
    public async Task Should_catch_418_while_get_comments()
    {
        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.GetCommentsAsync(new(1, 200, 1, "Id"), default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }

    [Fact]
    public async Task Should_catch_418_while_get_comment()
    {
        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.GetCommentAsync(_commentId, default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }

    [Fact]
    public async Task Should_catch_418_while_create_draft_comment()
    {
        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.CreateDraftCommentAsync(
                new(AUTHOR_ID, null, OBJECT_ID, OBJECT_TYPE_FULL_NAME), default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }

    [Fact]
    public async Task Should_catch_418_while_publish_comment()
    {
        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.PublishCommentAsync(_commentId, default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }

    [Fact]
    public async Task Should_catch_418_while_edit_comment_content()
    {
        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.EditCommentContentAsync(_commentId, "test-content", default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }

    [Fact]
    public async Task Should_catch_418_while_publish_new_comment()
    {
        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.PublishNewCommentAsync(new(AUTHOR_ID, "test-content", DateTime.UtcNow, OBJECT_ID, OBJECT_TYPE_FULL_NAME), default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }

    [Fact]
    public async Task Should_catch_418_while_get_object_id_to_comment_count_map()
    {
        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.GetObjectIdToCommentCountMapAsync(OBJECT_TYPE_FULL_NAME, new[] { OBJECT_ID }, default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }

    [Fact]
    public async Task Should_catch_418_while_add_comment_file()
    {
        using var fileContent = new MemoryStream(Encoding.UTF8.GetBytes("test"));

        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.AddCommentFileAsync(_commentId, new("test.txt", fileContent), default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }

    [Fact]
    public async Task Should_catch_418_while_get_comment_file()
    {
        using var fileContent = new MemoryStream(Encoding.UTF8.GetBytes("test"));

        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.GetCommentFileAsync(_commentId, _commentFileId, default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }

    [Fact]
    public async Task Should_catch_418_while_remove_comment_file()
    {
        using var fileContent = new MemoryStream(Encoding.UTF8.GetBytes("test"));

        var exception = await Assert.ThrowsAsync<FlurlHttpException>(() =>
        {
            return _client.RemoveCommentFileAsync(_commentId, _commentFileId, default);
        });

        Assert.Equal(EXPECTED_I_AM_TEAPOT, exception.StatusCode);
    }
}
