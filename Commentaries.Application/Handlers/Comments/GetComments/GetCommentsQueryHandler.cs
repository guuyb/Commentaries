using Commentaries.Domain.Models;
using Commentaries.Application.Common.Paging;
using Commentaries.Application.Ports;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Application.Handlers.Comments.GetComments;

public record GetCommentsQuery(
    int PageNumber,
    int PageSize,
    int Order,
    string Member,
    string? ObjectId = null,
    string? ObjectTypeFullName = null,
    CommentStateEnum? StateId = null,
    Guid[]? CommentIds = null) : IRequest<GetCommentsResultDto>, IPagingQuery
{
    public const int FIRST_PAGE_NUMBER = 1;
    public const int TAKE_ONE = 1;
    public const int ASCENDING_ORDER = 1;
}

internal sealed class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, GetCommentsResultDto>
{
    private readonly ICommentariesDbContext _commentDbContext;
    private readonly IValidator<GetCommentsQuery> _queryValidator;

    public GetCommentsQueryHandler(ICommentariesDbContext commentDbContext,
        IValidator<GetCommentsQuery> queryValidator)
    {
        _commentDbContext = commentDbContext;
        _queryValidator = queryValidator;
    }

    public async Task<GetCommentsResultDto> Handle(GetCommentsQuery query, CancellationToken cancellationToken)
    {
        await _queryValidator.ValidateOrThrowExceptionAsync(query, cancellationToken);

        var quaryableComments = _commentDbContext.Comments
            .Where(c => c.StateId != CommentStateEnum.Deleted);

        if (query.ObjectId is not null)
            quaryableComments = quaryableComments.Where(c => c.ObjectId == query.ObjectId);

        if (query.ObjectTypeFullName is not null)
            quaryableComments = quaryableComments.Where(c => c.ObjectType != null
                && c.ObjectType.FullName == query.ObjectTypeFullName);

        if (query.StateId is not null)
            quaryableComments = quaryableComments.Where(c => c.StateId == query.StateId);

        if (query.CommentIds is not null)
            quaryableComments = quaryableComments.Where(c => query.CommentIds.Contains(c.Id));

        var orderedComments = quaryableComments
            .Select(CommentDto.Selector)
            .OrderBy(query.Member, query.Order);

        var page = await orderedComments.PageAsync(query, cancellationToken);

        return new GetCommentsResultDto(
            page.Items.ToList(),
            page.TotalCount
        );
    }
}
