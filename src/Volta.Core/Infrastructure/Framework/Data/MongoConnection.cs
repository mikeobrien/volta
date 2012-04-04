using System;
using System.Linq;
using MongoDB.Driver;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public class MongoConnection : IConnection
    {
        private readonly Lazy<MongoServer> _mongo;

        public MongoConnection(string connectionString)
        {
            _mongo = new Lazy<MongoServer>(() => MongoServer.Create(connectionString));
            var connectionUri = new Uri(connectionString);
            var database = connectionUri.AbsolutePath.Substring(1);
            DefaultDatabase = string.IsNullOrEmpty(database) ? null : database;
            Server = connectionUri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || 
                connectionUri.Host == "127.0.0.1" ? Environment.MachineName : connectionUri.Host;
            var credentials = connectionUri.UserInfo;
            if (credentials.Contains(":")) Username = credentials.Split(':').First();
            ConnectionString = string.Format("mongodb://{0}@{1}/{2}", Username, Server, DefaultDatabase);
            ;
        }

        public MongoServer CreateConnection() { return _mongo.Value; }
        public string Server { get; private set; }
        public string Username { get; private set; }
        public string DefaultDatabase { get; private set; }
        public string ConnectionString{ get; private set; }
    }
}