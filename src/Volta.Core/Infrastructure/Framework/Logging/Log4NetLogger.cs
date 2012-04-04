using System;
using System.Linq;
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
            var fileAppender = GetAppender<log4net.Appender.FileAppender>();
            if (fileAppender != null) Path = fileAppender.File;
            var emailAppender = GetAppender<log4net.Appender.SmtpAppender>();
            if (emailAppender != null)
            {
                Email = emailAppender.To;
                SmtpServer = emailAppender.SmtpHost;
            }
        }

        private T GetAppender<T>()
        {
            return _logger.Logger.Repository.GetAppenders().OfType<T>().FirstOrDefault();
        }

        public void Write(Exception exception)
        {
            _logger.Error(exception);
        }

        public string Path { get; private set; }
        public string Email { get; private set; }
        public string SmtpServer { get; private set; }
    }
}