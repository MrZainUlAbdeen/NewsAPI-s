﻿using NewsBook.ModelDTO;
using NewsBook.Models;
using NewsBook.Models.Paging;

namespace NewsBook.Repository
{
    public interface INewsRepository : INewsBase<News>
    {
        Task<PagedList<News>> GetAll(NewsParameters newsParameters);
        Task<News> Insert(string Tittle, string Description);
        Task<News> Update(News news);
        Task<News> Delete(Guid Id);
        Task<List<News>> GetAll();
        Task<News?> GetById(Guid Id);
        Task<List<News>> GetFavouriteNews();
    }
}
