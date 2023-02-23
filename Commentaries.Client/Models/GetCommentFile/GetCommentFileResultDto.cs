using System;

namespace Commentaries.Client.Models.GetCommentFile;

public record GetCommentFileResultDto(FileDto File);

public record FileDto(Guid Id, string FileName, byte[] Content);
