using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsBook.Authorization;
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
        private readonly IMapper _mapper;

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
        public async Task<IActionResult> Get(
            [FromQuery] bool usePaging, 
            [FromQuery] PagingParameters pagingParameters
            )
        {
            if (usePaging == true)
            {
                var pagedNews = await _newsRepository.GetAll(pagingParameters);
                var pagedNewsDTO = new PagedList<NewsReadDTO>
                {
                    Items = _mapper.Map<List<NewsReadDTO>>(pagedNews.Items),
                    TotalCount = pagedNews.TotalCount,
                    TotalPages = pagedNews.TotalPages,
                    CurrentPage = pagedNews.CurrentPage,
                    PageSize = pagedNews.PageSize
                };
                return Ok(pagedNewsDTO);
            }
            var news = await _newsRepository.GetAll();
            return Ok(_mapper.Map<List<NewsReadDTO>>(news));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var getById = await _newsRepository.GetById(id);
            if (getById != null)
            {
                return Ok(getById);
            }
            else
            {
                return NotFound(id);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(NewsWriteDTO newsDTO)
        {
            var news = await _newsRepository.Insert(newsDTO.Tittle, newsDTO.Description);
            return Ok(_mapper.Map<NewsReadDTO>(news));
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] NewsWriteDTO newsDTO)
        {
            var news = await _newsRepository.GetById(id);
            if (news == null)
            {
                return BadRequest(id);
            }

            news.Title = newsDTO.Tittle;
            news.Description = newsDTO.Description;

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
        [Route("markfavourite")]
        public async Task<IActionResult> MarkFavourite([FromBody] FavouriteNewsWriteDTO favouriteNewsDTO)
        {
            var favouriteNews = await _favouriteNews.Insert(
                favouriteNewsDTO.NewsId,
                favouriteNewsDTO.IsFavourite
            );
            return Ok(_mapper.Map<FavouriteNewsReadDTO>(favouriteNews));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("favourite")]
        public async Task<IActionResult> GetFavouriteNews(
            [FromQuery] bool usePaging, 
            [FromQuery] PagingParameters pagingParameters
            )
        {
            if (usePaging == true)
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
                var pagedFavouriteNews = await _newsRepository.GetFavouriteNews(pagingParameters);
                var pagedNewsDTO = new PagedList<NewsReadDTO>
                {
                    Items = _mapper.Map<List<NewsReadDTO>>(pagedFavouriteNews.Items),
                    TotalCount = pagedFavouriteNews.TotalCount,
                    TotalPages = pagedFavouriteNews.TotalPages,
                    CurrentPage = pagedFavouriteNews.CurrentPage,
                    PageSize = pagedFavouriteNews.PageSize
                };
                return Ok(pagedNewsDTO);
            }
            var favouriteNews = await _newsRepository.GetFavouriteNews();
            return Ok(_mapper.Map<List<NewsReadDTO>>(favouriteNews));
        }
    }
}