using System.Linq;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public static class QueryableExtensions
    {
         public static PagedResult<T> GetPage<T>(this IQueryable<T> query, int pageSize, int page)
         {
             return new PagedResult<T>(query.Count(), query.Skip((page - 1) * pageSize).Take(pageSize));
         }

         public class PagedResult<TEntity>
         {
             public PagedResult(int totalRecords, IQueryable<TEntity> results)
             {
                 TotalRecords = totalRecords;
                 Results = results;
             }

             public int TotalRecords { get; private set; }
             public IQueryable<TEntity> Results { get; private set; }
         }
    }
}