using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniBlog.Api.Data.Entities;

namespace MiniBlog.Api.Data.EntityConfiguration;

public class TagEntityConfiguration : IEntityTypeConfiguration<TagEntity>
{
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder.HasKey(t => t.TagId);
        builder.Property(t => t.Name).IsRequired();

        builder.HasMany(t => t.Posts)
            .WithMany(p => p.Tags)
            .UsingEntity("PostsTags");
    }
}