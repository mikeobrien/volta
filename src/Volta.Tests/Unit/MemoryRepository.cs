using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Volta.Core.Infrastructure;
using Volta.Core.Infrastructure.Framework;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Unit
{
    public class MemoryRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IList<TEntity> _entites;
        private readonly IQueryable<TEntity> _query;

        public MemoryRepository(params TEntity[] entities)
        {
            _entites = new List<TEntity>(entities);
            _query = _entites.AsQueryable();
        }

        public TEntity Get(object id)
        {
            return _entites.FirstOrDefault(x => x.ToDynamic().Id == id);
        }

        public void Add(TEntity entity)
        {
            _entites.Add(entity);
        }

        public void Update(TEntity entity)
        {
            Delete(entity.ToDynamic().Id);
            Add(entity);
        }

        public void Delete(object id)
        {
            _entites.Where(x => x.ToDynamic().Id == id).Take(1).ToList().ForEach(x => _entites.Remove(x));
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            _query.Where(filter).ToList().ForEach(x => _entites.Remove(x));
        }

        public IEnumerator<TEntity> GetEnumerator() { return _entites.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _entites.GetEnumerator(); }
        public Type ElementType { get { return _query.ElementType; } }
        public Expression Expression { get { return _query.Expression; } }
        public IQueryProvider Provider { get { return _query.Provider; } }
    }
}