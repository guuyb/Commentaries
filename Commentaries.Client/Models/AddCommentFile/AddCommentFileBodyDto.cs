using System.IO;

namespace Commentaries.Client.Models.AddCommentFile;

public record AddCommentFileBodyDto(
    string FileName,
    Stream Content);
