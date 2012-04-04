using System;

namespace Volta.Core.Infrastructure.Application.Configuration
{
    public class Configuration : IConfiguration
    {
        readonly Lazy<ConfigurationSection> _configurationSection = new Lazy<ConfigurationSection>(
            () => (ConfigurationSection)System.Configuration.ConfigurationManager.GetSection("volta"));

        public string ConnectionString { get { return _configurationSection.Value.ConnectionString; } }
    }
}