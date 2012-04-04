using System;

namespace Volta.Core.Infrastructure.Application
{
    public interface ISystemInfo
    {
        string Version { get; }
        DateTime BuildDate { get; set; }
        SystemInfo.Mode DebugMode { get; }
        string WebServer { get; }
        string SmtpServer { get; }
        string Database { get; }
        string ErrorEmail { get; }
        string ErrorEmailSmtp { get; }
        string LogPath { get; }
    }
}