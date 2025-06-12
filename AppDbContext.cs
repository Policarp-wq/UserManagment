using Microsoft.EntityFrameworkCore;
using UserManagment.Models;

namespace UserManagment
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Guid);
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
        }
    }
}
