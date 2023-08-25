using Microsoft.EntityFrameworkCore;
using simpleApp.Enums;
using simpleApp.Models;

namespace simpleApp.DAL
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>();
            modelBuilder.Entity<User>();
        }
    }
}
