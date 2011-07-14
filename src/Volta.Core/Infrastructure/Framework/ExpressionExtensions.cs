using System;
using System.Linq.Expressions;

namespace Volta.Core.Infrastructure.Framework
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<TType, TValue>(this Expression<Func<TType, TValue>> property)
        {
            if (property.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("Mapping must be a property.", "property");
            return ((MemberExpression)property.Body).Member.Name;
        }
    }
}