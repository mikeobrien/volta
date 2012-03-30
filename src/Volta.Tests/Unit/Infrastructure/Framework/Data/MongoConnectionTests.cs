using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Unit.Infrastructure.Framework.Data
{
    [TestFixture]
    public class MongoConnectionTests
    {
        [Test] 
        public void should_parse_default_database()
        {
            var connection = new MongoConnection("mongodb://username:password@localhost/collection?yada");
            connection.DefaultDatabase.ShouldEqual("collection");
        }

        [Test]
        public void should_not_parse_default_database_if_not_specified()
        {
            var connection = new MongoConnection("mongodb://username:password@localhost?yada");
            connection.DefaultDatabase.ShouldBeNull();
        }
    }
}