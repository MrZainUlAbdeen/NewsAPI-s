
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
        //Friday
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
        //Today
        public IQueryable<T> GetFiltering<T>(string columnName, string value, IQueryable<T> source)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, columnName);
            var method = typeof(string).GetMethod("ToLower", System.Type.EmptyTypes);
            var toLower = Expression.Call(property, method);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsExpression = Expression.Call(toLower, containsMethod, Expression.Constant(value.ToLower()));
            var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
            var result = source.Where(lambda);
            return result;
        }



        public IQueryable<T> GetWithFilter(Expression<Func<T, bool>> predicates)
        {
            var source = _dbContext.Set<T>().AsQueryable();

            if (source != null)
            {
                source = source.Where(predicates);
            }
            return source;
        }

        public List<T> GetFiltering<T>(IQueryable<T> source, string columnName, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, columnName);
            var method = typeof(string).GetMethod("ToLower", System.Type.EmptyTypes);
            var toLower = Expression.Call(property, method);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsExpression = Expression.Call(toLower, containsMethod, Expression.Constant(value.ToLower()));
            var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
            var result = source.Where(lambda);
            return result.ToList();
        }

        public IQueryable<T> Search<T>(IQueryable<T> source, string searchTerm)
        {
            List<T> returnList = new List<T>();
            return (
                from obj in source
                where obj.Equals(searchTerm)
                select obj);
            //foreach (T t in source)
            //{
            //    source.Contains(t);
            //    //List<string> propertyValues = new List<string>();

            //    //foreach (PropertyInfo pi in typeof(T).GetProperties())
            //        //returnList.Add(pi.GetValue(t, null).ToString());

            //    if (returnList.Where(pv => pv. ToLower().Contains(searchTerm.ToLower())).Count() > 0)
            //        returnList.Add(t);
            //}

//            return returnList.AsQueryable();
        }
        //public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchTerm)
        //{
        //    string values =
        //        typeof(T).GetProperties().SelectMany(p => p.GetValue(T, null).ToString());

        //    return source.Where(values.Contains(searchTerm));
        //}
        public IQueryable<T> Filter(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> dbQuery = _dbContext.Set<T>();
            dbQuery = dbQuery.Where(filter);
            return dbQuery;
        }
    }
}