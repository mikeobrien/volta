using System.Configuration;

namespace Volta.Core.Application.Configuration
{
    public class ConfigurationSection : System.Configuration.ConfigurationSection
    {
        private const string ConnectionStringAttribute = "connectionString";
        private const string ErrorUrlAttribute = "errorUrl";
        private const string AccessDeniedUrlAttribute = "accessDeniedUrl";

        [ConfigurationProperty(ConnectionStringAttribute)]
        public string ConnectionString { get { return (string)this[ConnectionStringAttribute]; } }

        [ConfigurationProperty(ErrorUrlAttribute)]
        public string ErrorUrl { get { return (string)this[ErrorUrlAttribute]; } }

        [ConfigurationProperty(AccessDeniedUrlAttribute)]
        public string AccessDeniedUrl { get { return (string)this[AccessDeniedUrlAttribute]; } }
    }
}