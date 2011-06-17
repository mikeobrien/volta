using System.Configuration;

namespace Volta.Core.Application.Configuration
{
    public class ConfigurationSection : System.Configuration.ConfigurationSection
    {
        private const string ConnectionStringAttribute = "connectionString";

        [ConfigurationProperty(ConnectionStringAttribute)]
        public string ConnectionString
        {
            get { return (string)this[ConnectionStringAttribute]; }
        }
    }
}