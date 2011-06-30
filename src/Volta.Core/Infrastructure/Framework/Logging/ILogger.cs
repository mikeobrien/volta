using System;

namespace Volta.Core.Infrastructure.Framework.Logging
{
    public interface ILogger
    {
        void Log(string source, Exception exception);
    }
}