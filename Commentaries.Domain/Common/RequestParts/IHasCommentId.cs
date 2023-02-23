using System;

namespace Commentaries.Domain.Common.RequestParts;

public interface IHasCommentId
{
    Guid CommentId { get; }
}
