using Microsoft.EntityFrameworkCore;
using NewsBook.Data;

namespace NewsBook.Repository
{
    public class NewsBase<T> : IBaseRepository<T> where T : class
    {
        protected DatabaseContext _dbContext { get; set; }
        public NewsBase(DatabaseContext newsContext)
        {
            _dbContext = newsContext;
        }
        
        public IQueryable<T> FindAll()
        {

            return _dbContext.Set<T>().AsNoTracking();
        }
    }
}