using System;

namespace Commentaries.Domain.Handlers.Comments.GetCommentFile;

public record GetCommentFileResultDto(FileDto File);

public record FileDto(Guid Id, string FileName, byte[] Data);
