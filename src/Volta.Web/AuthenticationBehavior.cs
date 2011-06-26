using System;
using FubuCore;
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
        private readonly CurrentRequest _request;
        private readonly IOutputWriter _writer;
        private readonly IActionBehavior _actionBehavior;
        private readonly ISecureSession _secureSession;

        public AuthenticationBehavior(IUrlRegistry registry, CurrentRequest request, IOutputWriter writer, IActionBehavior actionBehavior, ISecureSession secureSession)
        {
            _registry = registry;
            _request = request;
            _writer = writer;
            _actionBehavior = actionBehavior;
            _secureSession = secureSession;
        }

        public void Invoke()
        {
            var loginPageUrl = _registry.UrlFor<LoginHandler>(x => x.Query(null));
            var onLoginPage = _request.Path.Equals(loginPageUrl, StringComparison.OrdinalIgnoreCase);
            var loggedIn = _secureSession.IsLoggedIn();

            if (loggedIn && onLoginPage) _writer.RedirectToUrl(_registry.UrlFor<DashboardHandler>(x => x.Query()));
            else if (loggedIn) _actionBehavior.Invoke();
            else if (onLoginPage) _actionBehavior.Invoke();
            else _writer.RedirectToUrl(loginPageUrl + "?RedirectUrl=" + _request.RawUrl.UrlEncode());
        }

        public void InvokePartial()
        {
            _actionBehavior.InvokePartial();
        }
    }
}