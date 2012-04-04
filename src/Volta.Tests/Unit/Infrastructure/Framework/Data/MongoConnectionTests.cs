using System;
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

        [Test]
        public void should_parse_remote_server_name()
        {
            var connection = new MongoConnection("mongodb://username:password@hal/collection?yada");
            connection.Server.ShouldEqual("hal");
        }

        [Test]
        public void should_parse_loopback_server_name()
        {
            var connection = new MongoConnection("mongodb://username:password@127.0.0.1/collection?yada");
            connection.Server.ShouldEqual(Environment.MachineName);
        }

        [Test]
        public void should_parse_local_host_server_name()
        {
            var connection = new MongoConnection("mongodb://username:password@localhost/collection?yada");
            connection.Server.ShouldEqual(Environment.MachineName);
        }

        [Test]
        public void should_parse_username()
        {
            var connection = new MongoConnection("mongodb://username:password@localhost/collection?yada");
            connection.Username.ShouldEqual("username");
        }

        [Test]
        public void should_return_passwordless_connection_string()
        {
            var connection = new MongoConnection("mongodb://username:password@hal/collection?yada");
            connection.ConnectionString.ShouldEqual("mongodb://username@hal/collection");
        }
    }
}