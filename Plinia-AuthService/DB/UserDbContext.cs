using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Plinia_AuthService.Models;

namespace Plinia_AuthService.DB;

public sealed class UserDbContext : IdentityDbContext<IdentityUser>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityUser>().ToTable("AspNetUsers");

        builder.Entity<IdentityUser>().Property(user => user.Email).IsUnicode();
        builder.Entity<IdentityUser>().Property(user => user.UserName).IsUnicode();
        builder.Entity<IdentityUser>().Property(user => user.Id).IsUnicode();
    }
}