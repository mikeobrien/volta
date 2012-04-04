using System;
using System.Linq;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public interface IRepository<TEntity> : IQueryable<TEntity> where TEntity : class
    {
        TEntity Get(Guid id);
        void Add(TEntity entity);
        void Replace(TEntity entity);
        void Modify(Guid id, dynamic entity);
        void Delete(Guid id);
        void Delete(TEntity entity);
    }
}