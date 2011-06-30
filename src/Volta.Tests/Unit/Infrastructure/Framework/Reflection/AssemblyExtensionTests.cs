using System.Reflection;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Reflection;

namespace Volta.Tests.Unit.Infrastructure.Framework.Reflection
{
    [TestFixture]
    public class AssemblyExtensionTests
    {
#if DEBUG
        [Test]
        public void Should_Be_In_Debug_Mode()
        {
            Assembly.GetExecutingAssembly().IsInDebugMode().ShouldBeTrue();
        }
#else
        [Test]
        public void Should_Be_In_Release_Mode()
        {
            Assembly.GetExecutingAssembly().IsInDebugMode().ShouldBeFalse();
        }
#endif
    }
}