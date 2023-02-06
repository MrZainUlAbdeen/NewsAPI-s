using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NewsBook.Models;

namespace NewsBook.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
           // builder.HasMany(user => user.favouriteNews).WithOne(favouriteNews => favouriteNews.User).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
