using AutoMapper;
using NewsBook.ModelDTO.News;
using NewsBook.Models;

namespace NewsBook.MappingProfiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsReadDTO>().ForMember(dest => dest.NewsId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
