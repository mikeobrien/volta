using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Norm;
using Norm.Collections;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IIdentityConvention _identityConvention;
        private readonly Lazy<IMongoCollection<TEntity>> _collection;
        private readonly Lazy<IQueryable<TEntity>> _query;

        public MongoRepository(MongoConnection mongoConnection, IIdentityConvention identityConvention)
        {
            _collection = new Lazy<IMongoCollection<TEntity>>(() => mongoConnection.Connection.GetCollection<TEntity>());
            _query = new Lazy<IQueryable<TEntity>>(() => _collection.Value.AsQueryable());
            _identityConvention = identityConvention;
        }

        public TEntity Get(object id)
        {
            return _collection.Value.FindOneByIdConvention(id, _identityConvention);
        }

        public void Add(TEntity entity)
        {
            _collection.Value.Insert(entity);
        }

        public void Update(TEntity entity)
        {
            _collection.Value.UpdateOneByIdConvention(entity, _identityConvention);
        }

        public void Delete(object id)
        {
            _collection.Value.DeleteByIdConvention(id, _identityConvention);
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            _query.Value.Where(filter).ToList().ForEach(x => _collection.Value.Delete(x));
        }

        public IEnumerator<TEntity> GetEnumerator() { return _query.Value.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _query.Value.GetEnumerator(); }
        public Type ElementType { get { return _query.Value.ElementType; } }
        public Expression Expression { get { return _query.Value.Expression; } }
        public IQueryProvider Provider { get { return _query.Value.Provider; } }
    }
}