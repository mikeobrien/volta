using Volta.Core.Application.Configuration;
using Volta.Tests.Common;
using NUnit.Framework;
using Should;

namespace Volta.Tests.Integration.Application
{
    [TestFixture]
    class ConfigurationTests
    {
        [Test]
        public void Should_Load_Configuration()
        {
            var manager = new Configuration();
            manager.ConnectionString.ShouldEqual(Constants.VoltaTestConnectionString);
        }
    }
}
