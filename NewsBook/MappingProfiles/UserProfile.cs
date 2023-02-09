using AutoMapper;
using NewsBook.Mediator.Response;
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
