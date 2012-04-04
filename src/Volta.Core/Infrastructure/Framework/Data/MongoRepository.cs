using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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

        private static readonly string IdPropertyName = 
            BsonClassMap.LookupConventions(typeof(TEntity)).IdMemberConvention.FindIdMember(typeof(TEntity));
        private static readonly PropertyInfo EntityIdProperty = typeof(TEntity).GetProperty(IdPropertyName);
        private static readonly Func<TEntity, Guid> GetEntityId = (entity) => (Guid)EntityIdProperty.GetValue(entity, null);

        private static readonly Func<Guid, IMongoQuery> IdQuery = id => Query.EQ("_id", id); 

        public MongoRepository(IConnection mongoConnection)
        {
            _collection = new Lazy<MongoCollection<TEntity>>(() =>
                mongoConnection.CreateConnection().
                    GetDatabase(mongoConnection.DefaultDatabase).
                    GetCollection<TEntity>(typeof(TEntity).Name));
            _query = new Lazy<IQueryable<TEntity>>(() => _collection.Value.AsQueryable<TEntity>());
        }

        public TEntity Get(Guid id)
        {
            return _collection.Value.FindOne(IdQuery(id));
        }

        public void Add(TEntity entity)
        {
            _collection.Value.Insert(entity);
        }

        public void Replace(TEntity entity)
        {
            _collection.Value.Update(IdQuery(GetEntityId(entity)), UpdateWrapper.Create(entity));
        }

        public void Modify(Guid id, object entity)
        {
            _collection.Value.Update(IdQuery(id), new UpdateDocument("$set", entity.ToBsonDocument()));
        }

        public void Delete(Guid id)
        {
            _collection.Value.Remove(IdQuery(id));
        }

        public void Delete(TEntity entity)
        {
            Delete(GetEntityId(entity));
        }

        public IEnumerator<TEntity> GetEnumerator() { return _query.Value.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _query.Value.GetEnumerator(); }
        public Type ElementType { get { return _query.Value.ElementType; } }
        public Expression Expression { get { return _query.Value.Expression; } }
        public IQueryProvider Provider { get { return _query.Value.Provider; } }
    }
}