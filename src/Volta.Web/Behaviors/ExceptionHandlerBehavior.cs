using System;
using System.Net;
using System.Reflection;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Reflection;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Web.Behaviors
{
    public class ExceptionHandlerBehavior : IActionBehavior
    {
        private readonly IActionBehavior _innerBehavior;
        private readonly IOutputWriter _outputWriter;
        private readonly ILogger _logger;
        private readonly IWebServer _webServer;

        public ExceptionHandlerBehavior(
            IActionBehavior innerBehavior,
            IOutputWriter outputWriter,
            ILogger logger,
            IWebServer webServer)
        {
            _innerBehavior = innerBehavior;
            _outputWriter = outputWriter;
            _logger = logger;
            _webServer = webServer;
            ReturnError = Assembly.GetExecutingAssembly().IsInDebugMode();
        }

        public bool ReturnError { get; set; }

        public void Invoke()
        {
            try
            {
                _innerBehavior.Invoke();
            }
            catch (Exception e)
            {
                _webServer.IgnoreErrorStatus = true;
                if (e is AuthorizationException)
                    _outputWriter.WriteResponseCode(HttpStatusCode.Forbidden, "You are not authorized to perform this action.");
                else
                {
                    _outputWriter.WriteResponseCode(HttpStatusCode.InternalServerError, "A system error has occured.");
                    _logger.Write(e);
                    if (ReturnError) _outputWriter.Write(MimeType.Text, e.ToString());
                }
            }
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}