using Microsoft.EntityFrameworkCore;
using StandardProject.Domain.Entities;

namespace StandardProject.Infrastructure.DataAccess;

public class StandardProjectDbContext : DbContext
{
    public StandardProjectDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StandardProjectDbContext).Assembly);
    }
}
