using Commentaries.Api.Models;
using Commentaries.Domain.Handlers.Comments.AddCommentFile;
using Commentaries.Domain.Handlers.Comments.CreateDraftComment;
using Commentaries.Domain.Handlers.Comments.EditCommentContent;
using Commentaries.Domain.Handlers.Comments.GetCommentFile;
using Commentaries.Domain.Handlers.Comments.GetComments;
using Commentaries.Domain.Handlers.Comments.GetObjectIdToCommentCountMap;
using Commentaries.Domain.Handlers.Comments.PublishComment;
using Commentaries.Domain.Handlers.Comments.PublishNewComment;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Api.Controllers;

[Route("api/comments")]
public partial class CommentsController : Controller
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<GetCommentsResultDto> GetComments(
        [FromQuery] GetCommentsQuery query,
        CancellationToken cancellation)
    {
        return _mediator.Send(query, cancellation);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetCommentResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("{commentId}")]
    public async Task<IActionResult> GetComment(
        [FromRoute] Guid commentId,
        CancellationToken cancellation)
    {
        var query = new GetCommentsQuery(
            GetCommentsQuery.FIRST_PAGE_NUMBER,
            GetCommentsQuery.TAKE_ONE,
            GetCommentsQuery.ASCENDING_ORDER,
            nameof(CommentDto.Id),
            CommentIds: new[] { commentId });
        var result = await _mediator.Send(query, cancellation);
        var comment = result.Comments.FirstOrDefault();

        if (comment is null)
        {
            return NotFound();
        }

        return Ok(new GetCommentResultDto(comment));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateDraftComment(
        [FromBody] CreateDraftCommentCommand command,
        CancellationToken cancellation)
    {
        var result = await _mediator.Send(command, cancellation);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("{CommentId}/publish")]
    public async Task<IActionResult> PublishComment(
        [FromRoute] PublishCommentCommand command,
        CancellationToken cancellation)
    {
        await _mediator.Send(command, cancellation);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("{commentId}")]
    public async Task<IActionResult> EditCommentContent(
        [FromRoute] Guid commentId,
        [FromBody] EditCommentContentBodyDto body,
        CancellationToken cancellation)
    {
        var command = new EditCommentContentCommand(
            CommentId: commentId,
            Content: body.Content);
        await _mediator.Send(command, cancellation);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("published")]
    public async Task<IActionResult> PublishComment(
        [FromBody] PublishNewCommentCommand command,
        CancellationToken cancellation)
    {
        var result = await _mediator.Send(command, cancellation);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("object-type/{objectTypeFullName}/counts")]
    public Task<GetObjectIdToCommentCountMapResultDto> GetObjectIdToCommentCountMap(
        [FromRoute] string objectTypeFullName,
        [FromQuery] GetObjectIdToCommentCountMapQueryDto requestQuery,
        CancellationToken cancellation)
    {
        var query = new GetObjectIdToCommentCountMapQuery(
            ObjectTypeFullName: objectTypeFullName,
            ObjectIds: requestQuery.ObjectIds
        );
        return _mediator.Send(query, cancellation);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("{commentId}/files")]
    public async Task<AddCommentFileResultDto> AddCommentFile(
        [FromRoute] Guid commentId,
        IFormFile file,
        CancellationToken cancellation)
    {
        using var fileContent = file.OpenReadStream();
        var command = new AddCommentFileCommand(commentId, file.FileName, fileContent);
        return await _mediator.Send(command, cancellation);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("{CommentId}/files/{CommentFileId}")]
    public Task<GetCommentFileResultDto> GetCommentFile(
        [FromRoute] GetCommentFileQuery query,
        CancellationToken cancellation)
    {
        return _mediator.Send(query, cancellation);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("{CommentId}/files/{CommentFileId}")]
    public async Task<IActionResult> RemoveCommentFile(
        [FromRoute] RemoveCommentFileCommand command,
        CancellationToken cancellation)
    {

        await _mediator.Send(command, cancellation);
        return Ok();
    }
}
