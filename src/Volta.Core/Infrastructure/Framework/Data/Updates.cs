using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public class Updates<TEntity>
    {
        private readonly Dictionary<MemberInfo, object> _values;

        public Updates(Dictionary<MemberInfo, object> values)
        {
            _values = values;
        }

        public Updates<TEntity> Set<TValue>(Expression<Func<TEntity, TValue>> member, TValue value)
        {
            _values.Add(((MemberExpression)member.Body).Member, value);
            return this;
        }
    }
}