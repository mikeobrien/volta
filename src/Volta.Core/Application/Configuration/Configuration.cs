using System;

namespace Volta.Core.Application.Configuration
{
    public class Configuration : IConfiguration
    {
        readonly Lazy<ConfigurationSection> _configurationSection = new Lazy<ConfigurationSection>(
            () => (ConfigurationSection)System.Configuration.ConfigurationManager.GetSection("volta"));

        public string ConnectionString { get { return _configurationSection.Value.ConnectionString; } }
        public string ErrorUrl { get { return _configurationSection.Value.ErrorUrl; } }
        public string AccessDeniedUrl { get { return _configurationSection.Value.AccessDeniedUrl; } }
    }
}