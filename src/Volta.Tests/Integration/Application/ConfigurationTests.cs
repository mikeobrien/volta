using Volta.Core.Application.Configuration;
using NUnit.Framework;
using Should;

namespace Volta.Tests.Integration.Application
{
    [TestFixture]
    class ConfigurationTests
    {
        [Test]
        public void should_load_configuration()
        {
            var manager = new Configuration();
            manager.ConnectionString.ShouldEqual("mongodb://yada/yada");
        }
    }
}
