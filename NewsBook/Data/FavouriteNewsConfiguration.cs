using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NewsBook.Models;
using System.Reflection.Emit;

namespace NewsBook.Data
{
    public class FavouriteNewsConfiguration : IEntityTypeConfiguration<FavouriteNews>
    {
        public void Configure(EntityTypeBuilder<FavouriteNews> builder)
        {
            builder.HasIndex(favouriteNews => new { favouriteNews.UserId, favouriteNews.NewsId }).IsUnique();
            builder.HasOne(favouriteNews => favouriteNews.User).WithMany(user => user.FavouriteNews).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(favouriteNews => favouriteNews.News).WithMany(news => news.FavouriteNews).OnDelete(DeleteBehavior.NoAction);
        }
    }
}