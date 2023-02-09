using AutoMapper;
using NewsBook.Models;

namespace NewsBook.MappingProfiles
{
    public class FavouriteNewsProfile : Profile
    {
        public FavouriteNewsProfile()
        {
            CreateMap<FavouriteNews, FavouriteNewsReadDTO>();
        }
    }
}
