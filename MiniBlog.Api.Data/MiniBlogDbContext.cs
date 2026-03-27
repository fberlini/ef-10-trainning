using Microsoft.EntityFrameworkCore;
using MiniBlog.Api.Data.Entities;

namespace MiniBlog.Api.Data;

public class MiniBlogDbContext(DbContextOptions<MiniBlogDbContext> options) : DbContext(options)
{
    public required DbSet<UserEntity> Users { get; set; }
    public required DbSet<PostEntity> Posts { get; set; }
    public required DbSet<TagEntity> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniBlogDbContext).Assembly);
    }
}