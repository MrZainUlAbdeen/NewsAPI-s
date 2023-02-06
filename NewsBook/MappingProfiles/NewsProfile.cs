using AutoMapper;
using NewsBook.ModelDTO.FavouriteNews;
using NewsBook.ModelDTO.News;
using NewsBook.ModelDTO.User;
using NewsBook.Models;

namespace NewsBook.MappingProfiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsReadDTO>().ForMember(dest => dest.NewsId, opt => opt.MapFrom(src => src.Id));
            CreateMap<FavouriteNews, FavouriteNewsReadDTO>();
            CreateMap<User, UserReadDTO>();
        }
    }
}
