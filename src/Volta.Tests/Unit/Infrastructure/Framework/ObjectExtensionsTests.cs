using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework;

namespace Volta.Tests.Unit.Infrastructure.Framework
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        private class SomeClass
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public override bool Equals(object obj) { return this.ObjectEquals(obj); }
            public override int GetHashCode() { return this.ObjectHashCode(Name, Age); }
        }

        [Test]
        public void Objects_Should_Have_The_Same_Hash_Code_And_Not_Be_Zero()
        {
            var a = new SomeClass { Age = 44, Name = "Wolfgang" };
            var b = new SomeClass { Age = 44, Name = "Wolfgang" };

            a.GetHashCode().ShouldNotEqual(0);
            a.GetHashCode().ShouldEqual(b.GetHashCode());
        }

        [Test]
        public void Objects_Should_Not_Have_The_Same_Hash_Code_And_Not_Be_Zero()
        {
            var a = new SomeClass { Age = 44, Name = "wolfgang" };
            var b = new SomeClass { Age = 44, Name = "Wolfgang" };

            a.GetHashCode().ShouldNotEqual(0);
            a.GetHashCode().ShouldNotEqual(b.GetHashCode());
        }

        [Test]
        public void Objects_Should_Be_Equal()
        {
            var a = new SomeClass { Age = 44, Name = "Wolfgang" };
            var b = new SomeClass { Age = 44, Name = "Wolfgang" };

            a.ShouldEqual(b);
        }

        [Test]
        public void Objects_Should_Not_Be_Equal()
        {
            var a = new SomeClass { Age = 44, Name = "wolfgang" };
            var b = new SomeClass { Age = 44, Name = "Wolfgang" };

            a.ShouldNotEqual(b);
        }
    }
}