
using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.Models;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NewsBook.Core
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected DatabaseContext _dbContext { get; set; }
        public BaseRepository(DatabaseContext newsContext)
        {
            _dbContext = newsContext;
        }

        public IQueryable<T> FindAll()
        {

            return _dbContext.Set<T>().AsNoTracking();
        }

        public  IQueryable<T> OrderByPropertyOrField<T>
            ( IQueryable<T> queryable, string propertyOrFieldName, bool ascending)
        {
            var elementType = typeof(T);
            var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

            var parameterExpression = Expression.Parameter(elementType);
            var propertyOrFieldExpression =
                Expression.PropertyOrField(parameterExpression, propertyOrFieldName);
            var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

            var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName,
                new[] { elementType, propertyOrFieldExpression.Type }, queryable.Expression, selector);

            return queryable.Provider.CreateQuery<T>(orderByExpression);
        }

        public IQueryable<T> GetWithFilter<T>
            (string columnName, string searchParameter, IQueryable<T> source)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, columnName);
            var method = typeof(string).GetMethod("ToLower", System.Type.EmptyTypes);
            var toLower = Expression.Call(property, method);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsExpression = Expression.Call(toLower, containsMethod, Expression.Constant(searchParameter.ToLower()));
            var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
            var result = source.Where(lambda);
            return result;
        }

        public IQueryable<T> GetWithFilterq(Expression<Func<T, bool>> predicates)
        {
            var source = _dbContext.Set<T>().Select(T => T).AsQueryable();

            if (source != null)
            {
                source = source.Where(predicates);
            }
            return source;
        }
    }
}