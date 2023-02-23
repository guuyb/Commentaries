using Commentaries.Data;
using Commentaries.Data.Models;
using Commentaries.Domain.Common.Extensions;
using Commentaries.Domain.Common.RequestParts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Domain.Handlers.Comments.GetObjectIdToCommentCountMap;

public record class GetObjectIdToCommentCountMapQuery(
    string[] ObjectIds,
    string ObjectTypeFullName) : IRequest<GetObjectIdToCommentCountMapResultDto>, IHasObjectTypeFullName;

internal sealed class GetObjectIdToCommentCountMapQueryHandler
    : IRequestHandler<GetObjectIdToCommentCountMapQuery, GetObjectIdToCommentCountMapResultDto>
{
    private readonly CommentariesContext _commentDbContext;
    private readonly IValidator<GetObjectIdToCommentCountMapQuery> _queryValidator;

    public GetObjectIdToCommentCountMapQueryHandler(CommentariesContext commentDbContext,
        IValidator<GetObjectIdToCommentCountMapQuery> queryValidator)
    {
        _commentDbContext = commentDbContext;
        _queryValidator = queryValidator;
    }

    public async Task<GetObjectIdToCommentCountMapResultDto> Handle(GetObjectIdToCommentCountMapQuery query,
        CancellationToken cancellation)
    {
        await _queryValidator
            .ValidateOrThrowExceptionAsync(query, cancellation);

        var objectIdToCommentCountMap = await _commentDbContext.Comments
            .Where(c => c.StateId == CommentStateEnum.Published)
            .Where(c => c.ObjectType != null && c.ObjectType.FullName == query.ObjectTypeFullName)
            .Where(c => query.ObjectIds.Contains(c.ObjectId))
            .GroupBy(c => c.ObjectId)
            .Select(g => new { g.Key, Count = g.Count() }) // due to ef limitation
            .ToDictionaryAsync(x => x.Key, x => x.Count, cancellation);

        return new GetObjectIdToCommentCountMapResultDto(
            ObjectIdToCommentCountMap: objectIdToCommentCountMap
        );
    }
}
