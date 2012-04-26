using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Volta.Core.Infrastructure.Framework.Logging;

namespace Volta.Web.Behaviors
{
    public class ExceptionHandlerBehavior : IActionBehavior
    {
        private readonly IActionBehavior _innerBehavior;
        private readonly ILogger _logger;
        private readonly IOutputWriter _outputWriter;

        public ExceptionHandlerBehavior(
            IActionBehavior innerBehavior,
            ILogger logger,
            IOutputWriter outputWriter)
        {
            _innerBehavior = innerBehavior;
            _logger = logger;
            _outputWriter = outputWriter;
        }

        public void Invoke()
        {
            try
            {
                _innerBehavior.Invoke();
            }
            catch (Exception e)
            {
                _logger.Write(e);
                _outputWriter.RedirectToUrl("/Error.html");
            }
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}