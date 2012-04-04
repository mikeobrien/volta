using System;
using System.IO;
using System.Reflection;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Reflection;

namespace Volta.Core.Infrastructure.Application
{
    public class SystemInfo : ISystemInfo
    {
        public enum Mode { Release, Debug }

        public SystemInfo(IConnection connection, ILogger logger)
        {
            var assembly = Assembly.GetExecutingAssembly();
            Version = assembly.GetName().Version.ToString();
            BuildDate = File.GetLastWriteTime(assembly.Location);
            DebugMode = assembly.IsInDebugMode() ? Mode.Debug : Mode.Release;
            WebServer = Environment.MachineName;
            SmtpServer = new System.Net.Mail.SmtpClient().Host;
            Database = connection.ConnectionString;
            ErrorEmail = logger.Email;
            ErrorEmailSmtp = logger.SmtpServer;
            LogPath = logger.Path;
        }

        public string Version { get; private set; }
        public DateTime BuildDate { get; set; }
        public Mode DebugMode { get; private set; }
        public string WebServer { get; private set; }
        public string SmtpServer { get; private set; }
        public string Database { get; private set; }
        public string ErrorEmail { get; private set; }
        public string ErrorEmailSmtp { get; private set; }
        public string LogPath { get; private set; }
    }
}