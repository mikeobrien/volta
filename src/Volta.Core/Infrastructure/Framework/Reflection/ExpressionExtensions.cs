using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Volta.Core.Infrastructure.Framework.Reflection
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TType, bool>> PropertyEquals<TType, TProperty>(
                        this Expression<Func<TType, TProperty>> expression, object value)
         {
             var member = expression.Body as MemberExpression;
             if (member == null) throw new Exception("Must be a member expression.");
             var property = member.Member as PropertyInfo;
             if (property == null) throw new Exception("Must be property access.");
             return Expression.Lambda<Func<TType, bool>>(
                        Expression.Equal(member, Expression.Constant(property.GetValue(value, null))),
                        expression.Parameters);
         }
    }
}