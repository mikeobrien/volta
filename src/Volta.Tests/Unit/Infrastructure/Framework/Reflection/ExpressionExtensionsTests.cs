using System;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Reflection;

namespace Volta.Tests.Unit.Infrastructure.Framework.Reflection
{
    [TestFixture]
    public class ExpressionExtensionsTests
    {
        public class SomeType { public string Name { get; set; } }

        [Test]
        public void Should_Return_Equal_Expression()
        {
            Expression<Func<SomeType, object>> expression = x => x.Name;
            var predicate = expression.PropertyEquals(new SomeType {Name = "bob"});

            predicate.Body.NodeType.ShouldEqual(ExpressionType.Equal);
            var equal = (BinaryExpression)predicate.Body;

            equal.Left.NodeType.ShouldEqual(ExpressionType.MemberAccess);
            var member = ((MemberExpression) equal.Left).Member;
            (member as PropertyInfo).ShouldNotBeNull();
            member.Name.ShouldEqual("Name");

            equal.Right.NodeType.ShouldEqual(ExpressionType.Constant);
            var value = ((ConstantExpression) equal.Right).Value;
            value.ShouldEqual("bob");
        }
    }
}