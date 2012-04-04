using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework;

namespace Volta.Tests.Unit.Infrastructure.Framework
{
    [TestFixture]
    public class FuncTests
    {
        [Test]
        public void lazy_should_execute_many_times()
        {
            var count = 0;
            var command = Func.Lazy(false, () => count++);
            count.ShouldEqual(0);
            command();
            count.ShouldEqual(1);
            command();
            count.ShouldEqual(2);
            command();
            count.ShouldEqual(3);
        }

        [Test]
        public void lazy_should_execute_only_once()
        {
            var count = 0;
            var command = Func.Lazy(true, () => count++);
            count.ShouldEqual(0);
            command();
            count.ShouldEqual(1);
            command();
            count.ShouldEqual(1);
            command();
            count.ShouldEqual(1);
        }

        [Test]
        public void memoize_should_only_used_cached_values()
        {
            var count = 0;
            var getValue = Func.Memoize<int, int>(x => ++count);
            getValue(33).ShouldEqual(1);
            getValue(33).ShouldEqual(1);
            count.ShouldEqual(1);
            getValue(89).ShouldEqual(2);
            getValue(89).ShouldEqual(2);
            count.ShouldEqual(2);
        }
    }
}