using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Application;

namespace Volta.Tests.Unit.Infrastructure.Application
{
    [TestFixture]
    public class ComparableModelBaseTests
    {
        private class SomeClass : ComparableModelBase
        {
            public string Name { get; set; }
            public int Age { get; set; }
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
