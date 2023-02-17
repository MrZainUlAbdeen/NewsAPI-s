using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace NewsBook.Application.Extensions
{
    public static class QueriableExtender
    {
        public static IQueryable<T> OrderByPropertyOrField<T>(this IQueryable<T> queryable, string propertyOrFieldName, bool ascending)
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
        //public static IQueryable<T> GetWithFilter(Expression<Func<T, bool>> predicates)
        //{
        //   var query = _dbContext.Set<T>().AsQueryable();

        //    if (query != null)
        //    {
        //        query = query.Where(predicates);
        //    }
        //    return query;
        //}
        //    public List<T> GetFiltering<T>(string columnName, string value, IQueryable<T> _data)
        //    {
        //        var parameter = Expression.Parameter(typeof(T), "x");
        //        var property = Expression.Property(parameter, columnName);
        //        var method = typeof(string).GetMethod("ToLower", System.Type.EmptyTypes);
        //        var toLower = Expression.Call(property, method);
        //        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        //        var containsExpression = Expression.Call(toLower, containsMethod, Expression.Constant(value.ToLower()));
        //        var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
        //        var result = _data.Where(lambda);
        //        return result.ToList();
        //    }
        //public static IQueryable<T> GetOrderInBetween<T>(this IQueryable<T> source,
        //string startTime,
        //string endTime)
        //{
        //    var elementType = typeof(T);
        //    var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

        //    var parameterExpression = Expression.Parameter(elementType);
        //    var propertyOrFieldExpression =
        //        Expression.PropertyOrField(parameterExpression, propertyOrFieldName);
        //    DateTime _startTime = DateTime.Parse(startTime);
        //    DateTime _endTime = DateTime.Parse(endTime);

        //    return _dbContext.Users.Where(a => a.CreatedAt >= _startTime
        //                                     && a.UpdatedAt <= _endTime);
        //    //Where(a => a.OrderTime >= _startTime
        //    //                          && a.OrderTime <= _endTime);
        //}



        //public static IEnumerable<T> FilteredData(IEnumerable<FilterParams> filterParams, IEnumerable<T> data)
        //{

        //    IEnumerable<string> distinctColumns = filterParams.Where(x => !String.IsNullOrEmpty(x.ColumnName)).Select(x => x.ColumnName).Distinct();

        //    foreach (string colName in distinctColumns)
        //    {
        //        var filterColumn = typeof(T).GetProperty(colName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
        //        if (filterColumn != null)
        //        {
        //            IEnumerable<FilterParams> filterValues = filterParams.Where(x => x.ColumnName.Equals(colName)).Distinct();

        //            if (filterValues.Count() > 1)
        //            {
        //                IEnumerable<T> sameColData = Enumerable.Empty<T>();

        //                foreach (var val in filterValues)
        //                {
        //                    sameColData = sameColData.Concat(FilterData(val.FilterOption, data, filterColumn, val.FilterValue));
        //                }

        //                data = data.Intersect(sameColData);
        //            }
        //            else
        //            {
        //                data = FilterData(filterValues.FirstOrDefault().FilterOption, data, filterColumn, filterValues.FirstOrDefault().FilterValue);
        //            }
        //        }
        //    }
        //    return data;
        //}
    }


}
