using System;
using System.Linq;
using System.Linq.Expressions;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public interface IRepository<TEntity> : IQueryable<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update<TKey>(Expression<Func<TEntity, TKey>> key, TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> filter);
    }
}