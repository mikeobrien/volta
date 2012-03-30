using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Wrappers;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly Lazy<MongoCollection<TEntity>> _collection;
        private readonly Lazy<IQueryable<TEntity>> _query;
        private static readonly PropertyInfo IdProperty = typeof (TEntity).GetProperty("Id");
        private static readonly Func<ObjectId, IMongoQuery> IdQuery = id => Query.EQ(IdProperty.Name, id); 

        public MongoRepository(MongoConnection mongoConnection)
        {
            _collection = new Lazy<MongoCollection<TEntity>>(() => 
                mongoConnection.Connection.
                    GetDatabase(mongoConnection.DefaultDatabase).
                    GetCollection<TEntity>(typeof(TEntity).Name));
            _query = new Lazy<IQueryable<TEntity>>(() => _collection.Value.AsQueryable<TEntity>());
        }

        public TEntity Get(ObjectId id)
        {
            return _collection.Value.FindOne(IdQuery(id));
        }

        public void Add(TEntity entity)
        {
            _collection.Value.Insert(entity);
        }

        public void Update<TKey>(TEntity entity)
        {
            _collection.Value.Update(IdQuery((ObjectId)IdProperty.GetValue(entity, null)), UpdateWrapper.Create(entity));
        }

        public void Delete(ObjectId id)
        {
            _collection.Value.Remove(IdQuery(id));
        }

        public IEnumerator<TEntity> GetEnumerator() { return _query.Value.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _query.Value.GetEnumerator(); }
        public Type ElementType { get { return _query.Value.ElementType; } }
        public Expression Expression { get { return _query.Value.Expression; } }
        public IQueryProvider Provider { get { return _query.Value.Provider; } }
    }
}