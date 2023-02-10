using AutoMapper;
using NewsBook.Models;
using NewsBook.Response;

namespace NewsBook.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponse>();
        }
         
    }
}
