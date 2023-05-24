using Common.Utils;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Guid);
            modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Guid = Guid.NewGuid(),
                    Login = "Admin",
                    PasswordHash = HashHelper.GetHash("Admin"),
                    Name = "Admin",
                    Admin = true,
                    CreatedOn = DateTime.UtcNow,
                });
        }

        public DbSet<User> Users => Set<User>();
    }
}