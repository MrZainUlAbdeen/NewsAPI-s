using NewsBook.Models;
using System.Linq.Expressions;

namespace NewsBook.Core
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> FindAll();
        //IQueryable<T> GetWithFilter(IQueryable<T> source, Expression<Func<T, bool>> predicate);
        public IQueryable<T> GetFiltering<T>(string columnName, string value, IQueryable<T> _data);
        IQueryable<T> OrderByPropertyOrField<T>(IQueryable<T> queryable, string propertyOrFieldName, bool ascending);
    }
}
