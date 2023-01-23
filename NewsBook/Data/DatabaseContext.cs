using Microsoft.EntityFrameworkCore;
using NewsBook.Models;

namespace NewsBook.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<News> News { get; set; }
        public DbSet<FavouriteNews> FavouriteNews { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
