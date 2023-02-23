using System;

namespace Commentaries.Domain.Common.RequestParts;

public interface IHasCommentFileId
{
    Guid CommentFileId { get; }
}
