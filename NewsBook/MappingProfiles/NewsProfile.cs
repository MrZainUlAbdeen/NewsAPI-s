using AutoMapper;
using NewsBook.Models;
using NewsBook.Response;

namespace NewsBook.MappingProfiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsResponse>().ForMember(dest => dest.NewsId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
