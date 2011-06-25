using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using Volta.Core.Application.Security;
using Volta.Web.Handlers;

namespace Volta.Web
{
    public class AuthenticationBehavior : IActionBehavior
    {
        private readonly IUrlRegistry _registry;
        private readonly IFubuRequest _request;
        private readonly IOutputWriter _writer;
        private readonly IActionBehavior _actionBehavior;
        private readonly ISecureSession _secureSession;

        public AuthenticationBehavior(IUrlRegistry registry, IFubuRequest request, IOutputWriter writer, IActionBehavior actionBehavior, ISecureSession secureSession)
        {
            _registry = registry;
            _request = request;
            _writer = writer;
            _actionBehavior = actionBehavior;
            _secureSession = secureSession;
        }

        public void Invoke()
        {
            var requestUrl = new Uri(_request.Get<CurrentRequest>().Path, UriKind.Relative);
            var loginUrl = new Uri(_registry.UrlFor<LoginHandler>(x => x.Query(null)), UriKind.Relative);
            if (!_secureSession.IsLoggedIn())
            {
                if (requestUrl == loginUrl) _actionBehavior.Invoke();
                else _writer.RedirectToUrl(loginUrl.ToString());
            }
            else
            {
                if (requestUrl == loginUrl) _writer.RedirectToUrl(_registry.UrlFor<DashboardHandler>(x => x.Query()));
                else _actionBehavior.Invoke(); 
            }
        }

        public void InvokePartial()
        {
            if (_secureSession.IsLoggedIn()) _actionBehavior.InvokePartial();
        }
    }
}