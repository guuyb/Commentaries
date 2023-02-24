using Commentaries.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commentaries.Infrastructure.SecondaryAdapters.Db.Configurations;

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
