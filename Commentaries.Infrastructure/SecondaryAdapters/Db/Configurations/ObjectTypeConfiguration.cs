using Commentaries.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commentaries.Infrastructure.SecondaryAdapters.Db.Configurations;

public class ObjectTypeConfiguration : IEntityTypeConfiguration<ObjectType>
{
    public void Configure(EntityTypeBuilder<ObjectType> builder)
    {
        builder.ToTable("ObjectType", "public");

        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.FullName)
            .IsUnique();

        builder.Property(p => p.FullName)
            .HasMaxLength(ObjectType.FULL_NAME_MAX_LENGTH)
            .IsRequired();

        builder.Property(p => p.CreatedDate)
            .HasUtcDateTimeConversion();
    }
}
