using System;
using System.Linq.Expressions;

namespace Volta.Core.Infrastructure
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<T>(this Expression<Func<T, object>> property)
        {
            if (property.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("Mapping must be a property.", "property");
            return ((MemberExpression)property.Body).Member.Name;
        }
    }
}