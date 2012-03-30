using System;
using MongoDB.Driver;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public class MongoConnection
    {
        private readonly Lazy<MongoServer> _mongo;

        public MongoConnection(string connectionString)
        {
            _mongo = new Lazy<MongoServer>(() => MongoServer.Create(connectionString));
            var database = new Uri(connectionString).AbsolutePath.Substring(1);
            DefaultDatabase = string.IsNullOrEmpty(database) ? null : database;
        }

        public MongoServer Connection { get { return _mongo.Value; } }
        public string DefaultDatabase { get; private set; }
    }
}