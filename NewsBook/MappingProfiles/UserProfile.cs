using AutoMapper;
using NewsBook.ModelDTO.User;
using NewsBook.Models;

namespace NewsBook.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDTO>();
        }
         
    }
}
