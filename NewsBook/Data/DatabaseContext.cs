using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using NewsBook.Models;
using System.Drawing;

namespace NewsBook.Data
{
    public class DatabaseContext : DbContextWithTriggers
    {
        private readonly IConfiguration Configuration;
        public DatabaseContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.Configuration = configuration;
        }
        public DbSet<News> News { get; set; }
        public DbSet<FavouriteNews> FavouriteNews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FavouriteNewsConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
