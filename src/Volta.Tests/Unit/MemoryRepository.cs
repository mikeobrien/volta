using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Reflection;

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
         
        public void Add(TEntity entity)
        {
            _entites.Add(entity);
        }

        public void Update<TType>(Expression<Func<TEntity, TType>> filter, TEntity entity)
        {
            Delete(filter.PropertyEquals(entity));
            Add(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> filter)
        {
            _entites.AsQueryable().Where(filter).Take(1).ToList().ForEach(x => _entites.Remove(x));
        }

        public IEnumerator<TEntity> GetEnumerator() { return _entites.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _entites.GetEnumerator(); }
        public Type ElementType { get { return _query.ElementType; } }
        public Expression Expression { get { return _query.Expression; } }
        public IQueryProvider Provider { get { return _query.Provider; } }
    }
}