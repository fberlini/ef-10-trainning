using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniBlog.Api.Data.Entities;

namespace MiniBlog.Api.Data.EntityConfiguration;

public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.HasKey(p => p.PostId);
        builder.Property(p => p.Title).IsRequired();
        builder.Property(p => p.Content).IsRequired();
        builder.Property(p => p.AuthorId).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired().HasDefaultValueSql("getutcdate()");
        builder.Property(p => p.UpdatedAt).IsRequired().HasDefaultValueSql("getutcdate()");

        builder.HasOne(p => p.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Tags)
            .WithMany(t => t.Posts)
            .UsingEntity("PostsTags");
    }
}