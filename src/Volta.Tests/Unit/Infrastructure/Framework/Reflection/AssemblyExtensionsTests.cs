using System.Reflection;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Reflection;

namespace Volta.Tests.Unit.Infrastructure.Framework.Reflection
{
    [TestFixture]
    public class AssemblyExtensionsTests
    {
#if DEBUG
        [Test]
        public void should_be_in_debug_mode()
        {
            Assembly.GetExecutingAssembly().IsInDebugMode().ShouldBeTrue();
        }
#else
        [Test]
        public void should_be_in_release_mode()
        {
            Assembly.GetExecutingAssembly().IsInDebugMode().ShouldBeFalse();
        }
#endif
    }
}