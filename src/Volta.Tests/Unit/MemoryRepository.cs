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

        public void Add(TEntity entity)
        {
            IdProperty.SetValue(entity, Guid.NewGuid(), null);
            _entites.Add(entity);
        }

        public void Replace(TEntity entity)
        {
            Delete(entity);
            Add(entity);
        }

        public void Modify(Guid id, object entity)
        {
            var entityToUpdate = Get(id);
            if (entityToUpdate != null) return;
            var values = entity.GetType().GetProperties().Where(x => x.CanRead && x.GetGetMethod().IsPublic).
                            Select(x => new { x.Name, Value = x.GetValue(entity, null)}).Union(
                         entity.GetType().GetFields().Where(x => x.IsPublic).
                            Select(x => new { x.Name, Value = x.GetValue(entity)}));
            foreach (var value in values)
            {
                var property = entityToUpdate.GetType().GetProperty(value.Name);
                if (property != null)
                {
                    property.SetValue(entityToUpdate, value.Value, null);
                    continue;
                }
                var field = entityToUpdate.GetType().GetField(value.Name);
                if (field != null) field.SetValue(entityToUpdate, value.Value);
            }
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