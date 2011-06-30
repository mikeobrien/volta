using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Web.Behaviors
{
    public class ExceptionHandlerBehavior : IActionBehavior
    {
        private readonly IOutputWriter _writer;
        private readonly IContentFile _contentFile;
        private readonly CurrentRequest _request;
        private readonly IActionBehavior _behavior;
        private readonly ILogger _logger;

        public ExceptionHandlerBehavior(IOutputWriter writer, IContentFile contentFile, CurrentRequest request, IActionBehavior behavior, ILogger logger)
        {
            _writer = writer;
            _contentFile = contentFile;
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
                _logger.Log(_request.RawUrl, e);
                _writer.Write(MimeType.Html.ToString(), _contentFile.ReadAllText("error.htm"));
            }
        }

        public void InvokePartial()
        {
            _behavior.InvokePartial();
        }
    }
}