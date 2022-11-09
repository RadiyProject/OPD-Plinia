using Microsoft.EntityFrameworkCore;
using Plinia_AuthService.Entities;

namespace Plinia_AuthService.DB;

public sealed class MySqlDbContext : DbContext
{
    public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users => Set<User>();
}