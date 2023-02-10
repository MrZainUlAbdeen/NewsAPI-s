using AutoMapper;
using NewsBook.Models;
using NewsBook.Response;

namespace NewsBook.MappingProfiles
{
    public class FavouriteNewsProfile : Profile
    {
        public FavouriteNewsProfile()
        {
            CreateMap<FavouriteNews, FavouriteNewsResponse>();
        }
    }
}
