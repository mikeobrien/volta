using System;

namespace Volta.Core.Infrastructure.Framework.Logging
{
    public interface ILogger
    {
        void Write(string source, Exception exception);
    }
}