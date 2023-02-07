using NewsBook.Models;

namespace NewsBook.IdentityServices
{
    public interface IIdentityServices
    {
        Guid? GetUserId();
    }
}
