using System;

namespace Commentaries.Application.Common.RequestParts;

public interface IHasCommentId
{
    Guid CommentId { get; }
}
