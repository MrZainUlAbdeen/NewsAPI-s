using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsBook.Authorization;
using NewsBook.Mediator.Commands.FavouriteNewsCommands;
using NewsBook.Mediator.Commands.News;
using NewsBook.Mediator.Queries.FavouriteNews;
using NewsBook.Mediator.Queries.News;
using NewsBook.Mediator.Response;
using NewsBook.Models.Paging;

namespace NewsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISender _mediator;

        public NewsController(
            IMapper mapper,
            ISender mediator
            )
        {
            _mediator= mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] bool usePaging, 
            [FromQuery] PagingParameters pagingParameters
            )
        {
            if (usePaging == true)
            {
                return Ok(await _mediator.Send(new GetPaginatedNewsQuery
                    { Page = pagingParameters }
                ));
            }
            var news = await _mediator.Send(new GetNewsQuery() );
            return Ok(_mapper.Map<List<NewsResponse>>(news));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new GetNewsByIdQuery { Id = id }));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(InsertNewsQuery insertNews)
        {
            var news = await _mediator.Send(insertNews);
            return Ok(news);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateNewsQuery updateNews)
        {
            return Ok(await _mediator.Send(updateNews)) ;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteNewsQuery { Id = id }));
        }

        [Authorize]
        [HttpPost]
        [Route("markfavourite{id}")]
        public async Task<IActionResult> MarkFavourite(Guid id)
        {
            return Ok(await _mediator.Send(new MarkFavouriteNews{ Id = id }));
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
                return Ok(await _mediator.Send(new GetPaginatedFavouriteNewsQuery
                {
                    Page = pagingParameters
                }
                ) );
            }
            return Ok(await _mediator.Send(new GetFavouriteNewsQuery()) );
        }

        [HttpDelete]
        [Route("favouritedelete{id}")]
        public async Task<IActionResult> DeleteFavouriteNews(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteFavouriteNews { Id = id }));
        }

    }
}