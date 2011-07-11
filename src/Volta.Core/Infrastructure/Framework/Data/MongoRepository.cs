using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Norm;
using Norm.Collections;
using Volta.Core.Infrastructure.Framework.Reflection;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly Lazy<IMongoCollection<TEntity>> _collection;
        private readonly Lazy<IQueryable<TEntity>> _query;

        public MongoRepository(MongoConnection mongoConnection)
        {
            _collection = new Lazy<IMongoCollection<TEntity>>(() => mongoConnection.Connection.GetCollection<TEntity>());
            _query = new Lazy<IQueryable<TEntity>>(() => _collection.Value.AsQueryable());
        }

        public void Add(TEntity entity)
        {
            _collection.Value.Insert(entity);
        }

        public void Update<TKey>(Expression<Func<TEntity, TKey>> key, TEntity entity)
        {
            var template = this.FirstOrDefault(key.PropertyEquals(entity));
            if (template != null) _collection.Value.UpdateOne(template, entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> filter)
        {
            var template = this.FirstOrDefault(filter);
            if (template != null) _collection.Value.Delete(template);
        }

        public IEnumerator<TEntity> GetEnumerator() { return _query.Value.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _query.Value.GetEnumerator(); }
        public Type ElementType { get { return _query.Value.ElementType; } }
        public Expression Expression { get { return _query.Value.Expression; } }
        public IQueryProvider Provider { get { return _query.Value.Provider; } }
    }
}