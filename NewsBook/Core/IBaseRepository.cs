using NewsBook.Models;
using System.Linq.Expressions;

namespace NewsBook.Core
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> FindAll();
        public IQueryable<T> GetWithFilter<T>(string columnName, string searchParameter, IQueryable<T> source);
        IQueryable<T> OrderByPropertyOrField<T>(IQueryable<T> queryable, string propertyOrFieldName, bool ascending);
    }
}
