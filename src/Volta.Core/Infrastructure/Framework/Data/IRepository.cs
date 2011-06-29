using System;
using System.Linq;
using System.Linq.Expressions;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public interface IRepository<TEntity> : IQueryable<TEntity> where TEntity : class
    {
        TEntity Get(object id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        void DeleteMany(Expression<Func<TEntity, bool>> filter);
    }
}