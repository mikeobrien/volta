using System;

namespace Volta.Core.Infrastructure.Framework.Logging
{
    public interface ILogger
    {
        void Write(Exception exception);
        string Email { get; }
        string SmtpServer { get; }
        string Path { get; }
    }
}