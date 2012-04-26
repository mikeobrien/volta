using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Unit
{
    public class MemoryRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IList<TEntity> _entites;
        private readonly IQueryable<TEntity> _query;
        private static readonly PropertyInfo IdProperty = typeof(TEntity).GetProperty("Id");
        private static readonly Func<TEntity, Guid> GetId = entity => (Guid) IdProperty.GetValue(entity, null);

        public MemoryRepository(params TEntity[] entities)
        {
            _entites = new List<TEntity>(entities);
            _query = _entites.AsQueryable();
        }
         
        public TEntity Get(Guid id)
        {
            return _entites.FirstOrDefault(x => GetId(x) == id);
        }

        public TEntity Add(TEntity entity)
        {
            IdProperty.SetValue(entity, Guid.NewGuid(), null);
            _entites.Add(entity);
            return entity;
        }

        public void Replace(TEntity entity)
        {
            Delete(entity);
            _entites.Add(entity);
        }

        public void Modify(Guid id, Action<Updates<TEntity>> updates)
        {
            var values = new Dictionary<MemberInfo, object>();
            updates(new Updates<TEntity>(values));
            var entity = Get(id);
            values.ToList().ForEach(x => {
                if (x.Key.MemberType == MemberTypes.Property)
                    ((PropertyInfo) x.Key).SetValue(entity, x.Value, null);
                else ((FieldInfo)x.Key).SetValue(entity, x.Value);
            });
        }

        public void Delete(Guid id)
        {
            var entity = Get(id);
            if (entity != null) _entites.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            Delete(GetId(entity));
        }

        public IEnumerator<TEntity> GetEnumerator() { return _entites.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _entites.GetEnumerator(); }
        public Type ElementType { get { return _query.ElementType; } }
        public Expression Expression { get { return _query.Expression; } }
        public IQueryProvider Provider { get { return _query.Provider; } }
    }
}