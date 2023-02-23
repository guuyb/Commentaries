using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace Commentaries.Data.Models;

public class CommentState
{
    public const int CODE_MAX_LENGTH = 50;

    public CommentStateEnum Id { get; set; }
    public string Code { get; set; } = string.Empty;
}

public class CommentStateConfiguration : IEntityTypeConfiguration<CommentState>
{
    public void Configure(EntityTypeBuilder<CommentState> builder)
    {
        builder.ToTable("CommentState", "public");
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasConversion<int>();

        builder.Property(p => p.Code)
            .HasMaxLength(CommentState.CODE_MAX_LENGTH)
            .IsRequired();

        builder.HasData(
            Enum.GetValues(typeof(CommentStateEnum))
                .Cast<CommentStateEnum>()
                .Select(s => new CommentState()
                {
                    Id = s,
                    Code = s.ToString(),
                }));
    }
}
