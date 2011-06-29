using System;
using Norm;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public class MongoConnection
    {
        private readonly Lazy<IMongo> _mongo;

        public MongoConnection(string connectionString)
        {
            _mongo = new Lazy<IMongo>(() => Mongo.Create(connectionString));
        }

        public IMongo Connection { get { return _mongo.Value; } }
    }
}