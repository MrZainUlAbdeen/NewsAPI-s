using AutoMapper;
using NewsBook.Mediator.Response;
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
