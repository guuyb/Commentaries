using Commentaries.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Commentaries.Application.Handlers.Comments.GetComments;

public record GetCommentsResultDto(List<CommentDto> Comments, long TotalCount);

public class CommentDto
{
    public Guid Id { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public string? Content { get; set; }
    public CommentStateEnum StateId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? PublishedDate { get; set; }
    public FileDto[] Files { get; set; } = Array.Empty<FileDto>();

    static internal Expression<Func<Comment, CommentDto>> Selector => c =>
        new CommentDto
        {
            Id = c.Id,
            AuthorId = c.AuthorId,
            Content = c.Content,
            StateId = c.StateId,
            CreatedDate = c.CreatedAt,
            PublishedDate = c.PublishedAt,
            Files = c.Files.AsQueryable()
                .Where(f => !f.IsDeleted)
                .Select(FileDto.Selector)
                .ToArray(),
        };
}

public record FileDto(Guid Id, string FileName)
{
    static internal Expression<Func<CommentFile, FileDto>> Selector => f =>
        new(f.Id, f.FileName);
}
