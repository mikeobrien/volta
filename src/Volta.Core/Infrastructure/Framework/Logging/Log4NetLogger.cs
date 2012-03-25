using System;
using log4net;
using log4net.Config;

namespace Volta.Core.Infrastructure.Framework.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _logger;

        public Log4NetLogger()
        {
            _logger = LogManager.GetLogger("Volta");
            XmlConfigurator.Configure();
        }

        public void Write(Exception exception)
        {
            _logger.Error(exception);
        }
    }
}