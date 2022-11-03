using Microsoft.EntityFrameworkCore;
using plinia.Models;

namespace plinia.dbcontexts
{
    public class UsersContexts : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersContexts()
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
            User adminUser1 = new User { id = 1, email = "admin@mail.com", password = "123456" };
            User adminUser2 = new User { id = 2, email = "tom@mail.com", password = "123456" };
            User simpleUser1 = new User { id = 3, email = "bob@mail.com", password = "123456" };
            User simpleUser2 = new User { id = 4, email = "sam@mail.com", password = "123456" };

            modelBuilder.Entity<User>().HasData(new User[] { adminUser1, adminUser2, simpleUser1, simpleUser2 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
