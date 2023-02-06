using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsBook.Authorization;
using NewsBook.IdentityServices;
using NewsBook.ModelDTO.FavouriteNews;
using NewsBook.ModelDTO.FavouriteNewsReadDTO;
using NewsBook.ModelDTO.News;
using NewsBook.Models;
using NewsBook.Models.Paging;
using NewsBook.Repository;

namespace NewsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;
        private readonly IFavouriteNewsRespository _favouriteNews;
        private IMapper _mapper;

        public NewsController(
            INewsRepository newsRepository, 
            IFavouriteNewsRespository favouriteNews,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _newsRepository = newsRepository;
            _favouriteNews = favouriteNews;
        }
        [HttpGet]
        
        public async Task<IActionResult> Get([FromQuery] NewsParameters newsParameters)
        {
            var _news = await _newsRepository.GetAll(newsParameters);
            return Ok(_news);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            var getById = await _newsRepository.GetById(Id);
            if (getById != null)
            {
                return Ok(getById);
            }
            else
            {
                return NotFound(Id);
            }
        }
        [Authorize]
        //Post News
        [HttpPost]
        public async Task<IActionResult> Post(NewsWriteDTO newsDTO)
        {
            var news = await _newsRepository.Insert(newsDTO.Tittle, newsDTO.Description);
            return Ok(_mapper.Map<NewsReadDTO>(news));
        }
        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IActionResult> Put(Guid Id, [FromBody] NewsWriteDTO NewsDTO)
        {

            var news = await _newsRepository.GetById(Id);
            if (news == null)
            {
                return BadRequest(Id);
            }

            news.Title = NewsDTO.Tittle;
            news.Description = NewsDTO.Description;

            news = await _newsRepository.Update(news);
            return Ok(news);
        }
        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var news = await _newsRepository.GetById(Id);
            if (news == null)
            {
                return BadRequest(Id);
            }

            return Ok(await _newsRepository.Delete(Id));
        }
        [Authorize]
        [HttpPost]
        [Route("MarkFavourite")]
        public async Task<IActionResult> MarkFavourite([FromBody] FavouriteNewsWriteDTO FavouriteNewsDTO)
        {
            var favouriteNews = await _favouriteNews.Insert(
                FavouriteNewsDTO.NewsId,
                FavouriteNewsDTO.IsFavourite
            );
            return Ok(_mapper.Map<FavouriteNewsReadDTO>(favouriteNews));
        }
        
        [Authorize]
        [HttpGet]
        [Route("Favourite")]
        public async Task<IActionResult> GetFavouriteNews()
        {
            //NOTE: Solution to map object to another object
            //var news = (await _newsRepository.GetFavouriteNews())
            //    .Select(
            //        newsDTO => new NewsReadDTO
            //        {
            //            NewsId = newsDTO.Id,
            //            UserId = newsDTO.UserId,
            //            Title = newsDTO.Title,
            //            Description = newsDTO.Description,
            //            CreatedAt = newsDTO.CreatedAt,
            //            UpdatedAt = newsDTO.UpdatedAt
            //        }
            //    );
            //return Ok(news);
            var news = (await _newsRepository.GetFavouriteNews());
            return Ok(_mapper.Map<List<NewsReadDTO>>(news));
        }
    }
}
