using Microsoft.EntityFrameworkCore;
using NewsBook.Data;

namespace NewsBook.Repository
{
    public class NewsBase<T> : INewsBase<T> where T : class
    {
        protected DatabaseContext newsContext { get; set; }
        public NewsBase(DatabaseContext newsContext)
        {
            this.newsContext = newsContext;
        }
        
        public IQueryable<T> FindAll()
        {

            return this.newsContext.Set<T>().AsNoTracking();
        }
    }
}