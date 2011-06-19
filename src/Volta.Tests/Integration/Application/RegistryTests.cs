using NUnit.Framework;
using Volta.Web;
using StructureMap;

namespace Volta.Tests.Integration.Application
{
    [TestFixture]
    public class RegistryTests
    {
        [Test]
        public void Should_Resolve_Types()
        {
            new Container(new Registry()).AssertConfigurationIsValid();
        }
    }
}