using System;
using System.Linq.Expressions;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using Volta.Core.Application.Security;

namespace Volta.Web.Behaviors
{
    public class SecureRequest<TLoginHandler, TDefaultHandler>
    {        
        private readonly IUrlRegistry _registry;
        private readonly CurrentRequest _request;
        private readonly ISecureSession _secureSession;
        private readonly Expression<Action<TLoginHandler>> _loginAction;
        private readonly Expression<Action<TDefaultHandler>> _defaultAction;

        public SecureRequest(IUrlRegistry registry, 
                             CurrentRequest request, 
                             ISecureSession secureSession,
                             Expression<Action<TLoginHandler>> loginAction,
                             Expression<Action<TDefaultHandler>> defaultAction)
        {
            _registry = registry;
            _request = request;
            _loginAction = loginAction;
            _secureSession = secureSession;
            _defaultAction = defaultAction;
        }

        public bool IsLoggedIn() { return _secureSession.IsLoggedIn(); }

        public string Url() { return _request.RawUrl; }

        public string LoginPageUrl() { return _registry.UrlFor(_loginAction); }
        public string DefaultPageUrl() { return _registry.UrlFor(_defaultAction); }

        public bool IsOnLoginPage() { return _request.Path.Equals(LoginPageUrl(), StringComparison.OrdinalIgnoreCase); }
        public bool IsOnDefaultPage() { return _request.Path.Equals(DefaultPageUrl(), StringComparison.OrdinalIgnoreCase); }
    }
}