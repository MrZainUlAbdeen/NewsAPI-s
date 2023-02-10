using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using System.Linq.Expressions;

namespace NewsBook.Core
{
    public class BaseResponse<T> : IBaseResponse<T> where T : class
    {
        protected DatabaseContext _dbContext { get; set; }
        public BaseResponse(DatabaseContext newsContext)
        {
            _dbContext = newsContext;
        }

        public IQueryable<T> FindAll()
        {

            return _dbContext.Set<T>().AsNoTracking();
        }
        //public static IQueryable<T> OrderBy<T> (IQueryable<T> source, string orderBy, bool isAscending=true)
        //{
        //    var elementType = typeof(T);
        //    var orderByMethodName = isAscending ? "OrderBy" : "OrderByDescending";
        //    var parameterExpression = Expression.Parameter(elementType);

        //    if (isAscending)
        //    {
        //        return source.OrderBy(
        //            item => typeof(T)
        //            .GetProperty(orderBy)
        //            .GetValue(item)
        //        );
        //    }
        //    return source.OrderByDescending(
        //        item => typeof(T)
        //        .GetProperty(orderBy)
        //        .GetValue(item)
        //    );
        //}
    }
}