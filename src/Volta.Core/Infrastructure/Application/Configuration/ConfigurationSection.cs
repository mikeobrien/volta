using System.Configuration;

namespace Volta.Core.Infrastructure.Application.Configuration
{
    public class ConfigurationSection : System.Configuration.ConfigurationSection
    {
        private const string ConnectionStringAttribute = "connectionString";
        private const string FileStorePathAttribute = "fileStorePath";

        [ConfigurationProperty(ConnectionStringAttribute)]
        public string ConnectionString { get { return (string)this[ConnectionStringAttribute]; } }

        [ConfigurationProperty(FileStorePathAttribute)]
        public string FileStorePath { get { return (string)this[FileStorePathAttribute]; } }
    }
}