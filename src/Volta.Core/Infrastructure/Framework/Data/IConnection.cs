using MongoDB.Driver;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public interface IConnection
    {
        MongoServer CreateConnection();
        string Server { get; }
        string Username { get; }
        string DefaultDatabase { get; }
        string ConnectionString { get; }
    }
}