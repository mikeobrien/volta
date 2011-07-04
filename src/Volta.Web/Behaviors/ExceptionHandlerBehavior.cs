using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Volta.Core.Infrastructure.Framework.Logging;

namespace Volta.Web.Behaviors
{
    public class ExceptionHandlerBehavior : IActionBehavior
    {
        private readonly IOutputWriter _writer;
        private readonly CurrentRequest _request;
        private readonly IActionBehavior _behavior;
        private readonly ILogger _logger;

        public ExceptionHandlerBehavior(IOutputWriter writer, CurrentRequest request, IActionBehavior behavior, ILogger logger)
        {
            _writer = writer;
            _request = request;
            _behavior = behavior;
            _logger = logger;
        }

        public void Invoke()
        {
            try
            {
                _behavior.Invoke();
            }
            catch (Exception e)
            {
                _logger.Write(_request.RawUrl, e);
                _writer.RedirectToUrl("/content/error.htm");
            }
        }

        public void InvokePartial()
        {
            _behavior.InvokePartial();
        }
    }
}