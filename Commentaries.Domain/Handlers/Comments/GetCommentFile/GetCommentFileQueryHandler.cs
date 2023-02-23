using Commentaries.Data;
using Commentaries.Data.Models;
using Commentaries.Domain.Common;
using Commentaries.Domain.Common.RequestParts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Domain.Handlers.Comments.GetCommentFile;
public record GetCommentFileQuery(
    Guid CommentId,
    Guid CommentFileId) : IRequest<GetCommentFileResultDto>, IHasCommentId, IHasCommentFileId;

internal class GetCommentFileQueryHandler :
    IRequestHandler<GetCommentFileQuery, GetCommentFileResultDto>
{
    private readonly CommentariesContext _context;
    private readonly IValidator<GetCommentFileQuery> _queryValidator;

    public GetCommentFileQueryHandler(
        CommentariesContext context,
        IValidator<GetCommentFileQuery> queryValidator)
    {
        _context = context;
        _queryValidator = queryValidator;
    }
    public async Task<GetCommentFileResultDto> Handle(
        GetCommentFileQuery query,
        CancellationToken cancellation)
    {
        await _queryValidator.ValidateOrThrowExceptionAsync(query, cancellation);

        var fileOrNull = await _context.CommentFiles
            .Where(f => f.Id == query.CommentFileId)
            .Where(f => f.Comment != null 
                && f.Comment.Id == query.CommentId
                && f.Comment.StateId != CommentStateEnum.Deleted)
            .FirstOrDefaultAsync(cancellation);

        fileOrNull.ThrowNotFoundIfNull(query.CommentFileId);
        var file = fileOrNull!;

        return new GetCommentFileResultDto(
            new FileDto(
                Id: file.Id,
                FileName: file.FileName,
                Data: file.Data));
    }
}
