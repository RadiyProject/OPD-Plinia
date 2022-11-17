using Microsoft.EntityFrameworkCore;
using NewServer.Models;

namespace NewServer.DBContext
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration Configuration = Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            string connection = Configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseMySql(connection,
                ServerVersion.AutoDetect(connection));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.name).IsRequired();
                entity.Property(e => e.email).IsRequired();
                entity.Property(e => e.password).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
