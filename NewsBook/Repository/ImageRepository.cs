using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.IdentityServices;
using NewsBook.Models;

namespace NewsBook.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly IIdentityServices _identityServices;
        private readonly DatabaseContext _dbContext;
        public ImageRepository(IIdentityServices identity, DatabaseContext context)
        {
            _dbContext= context ?? throw new ArgumentNullException(nameof(context));
            _identityServices = identity;
        }
        public async Task<string> Add(string image)
        {
            Guid loggedInUserId = _identityServices.GetUserId() ?? Guid.Empty;
            var picture = new Picture()
            {
                Profile = image,
                UserId = loggedInUserId
            };
            await Add(picture);
            return image;
        }
        private async Task<Picture> GetById(Guid id)
        {
            var picture = await _dbContext.Pictures.FindAsync(id);
            return picture;
        }
        private async Task<Picture> Add(Picture picture)
        {
            _dbContext.Pictures.Add(picture);
            await _dbContext.SaveChangesAsync();
            return picture;
        }
    }
}
