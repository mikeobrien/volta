using System;

namespace Volta.Core.Application.Configuration
{
    public class Configuration : IConfiguration
    {
        readonly Lazy<ConfigurationSection> _configurationSection = new Lazy<ConfigurationSection>(
            () => (ConfigurationSection)System.Configuration.ConfigurationManager.GetSection("Volta"));

        public string ConnectionString
        {
            get { return _configurationSection.Value.ConnectionString; }
        }
    }
}