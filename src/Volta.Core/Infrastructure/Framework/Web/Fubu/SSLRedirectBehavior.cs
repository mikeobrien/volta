using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;

namespace Volta.Core.Infrastructure.Framework.Web.Fubu
{
    public class SSLRedirectBehavior : IActionBehavior
    {
        private readonly IActionBehavior _innerBehavior;
        private readonly IOutputWriter _outputWriter;
        private readonly ICurrentHttpRequest _request;

        public SSLRedirectBehavior(IActionBehavior innerBehavior, IOutputWriter outputWriter, ICurrentHttpRequest request)
        {
            _innerBehavior = innerBehavior;
            _outputWriter = outputWriter;
            _request = request;
        }

        public void Invoke()
        {
            var url = _request.FullUrl();
            if (url.StartsWith("https://")) _innerBehavior.Invoke();
            else _outputWriter.RedirectToUrl(url.Replace("http://", "https://"));
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}