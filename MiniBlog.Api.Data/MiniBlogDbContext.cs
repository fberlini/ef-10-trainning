using Microsoft.EntityFrameworkCore;
using MiniBlog.Api.Data.EntityConfiguration;
using MiniBlog.Api.Data.Entities;

namespace MiniBlog.Api.Data;

public class MiniBlogDbContext(DbContextOptions<MiniBlogDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<TagEntity> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniBlogDbContext).Assembly);
    }
}