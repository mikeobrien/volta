using System.Linq;
using MongoDB.Bson;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public interface IRepository<TEntity> : IQueryable<TEntity> where TEntity : class
    {
        TEntity Get(ObjectId id);
        void Add(TEntity entity);
        void Update<TKey>(TEntity entity);
        void Delete(ObjectId id);
    }
}