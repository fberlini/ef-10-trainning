using Microsoft.EntityFrameworkCore;
using MiniBlog.Api.Data.Entities;

namespace MiniBlog.Api.Data;

public class MiniBlogDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Initial catalog=MiniBlogDB;User ID=sa;Password=SuperSecret123;Encrypt=False;TrustServerCertificate=True;Connection Timeout=3;");
    }
    
}