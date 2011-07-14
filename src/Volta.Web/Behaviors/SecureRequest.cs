using System;
using System.Linq.Expressions;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Behaviors
{
    public class SecureRequest<TLoginHandler, TLogoutHandler, TDefaultHandler>
    {        
        private readonly IUrlRegistry _registry;
        private readonly CurrentRequest _request;
        private readonly ISecureSession<Token> _secureSession;
        private readonly Expression<Action<TLoginHandler>> _loginAction;
        private readonly Expression<Action<TLogoutHandler>> _logoutAction;
        private readonly Expression<Action<TDefaultHandler>> _defaultAction;

        public SecureRequest(IUrlRegistry registry, 
                             CurrentRequest request,
                             ISecureSession<Token> secureSession,
                             Expression<Action<TLoginHandler>> loginAction,
                             Expression<Action<TLogoutHandler>> logoutAction,
                             Expression<Action<TDefaultHandler>> defaultAction)
        {
            _registry = registry;
            _request = request;
            _loginAction = loginAction;
            _logoutAction = logoutAction;
            _secureSession = secureSession;
            _defaultAction = defaultAction;
        }

        public bool IsLoggedIn() { return _secureSession.IsLoggedIn(); }

        public string Url() { return _request.RawUrl; }

        public string LoginPageUrl() { return _registry.UrlFor(_loginAction); }
        public string DefaultPageUrl() { return _registry.UrlFor(_defaultAction); }
        public string LogoutPageUrl() { return _registry.UrlFor(_logoutAction); }

        public bool IsOnLoginPage() { return _request.Path.Equals(LoginPageUrl(), StringComparison.OrdinalIgnoreCase); }
        public bool IsOnDefaultPage() { return _request.Path.Equals(DefaultPageUrl(), StringComparison.OrdinalIgnoreCase); }
        public bool IsOnLogoutPage() { return _request.Path.Equals(LogoutPageUrl(), StringComparison.OrdinalIgnoreCase); }
    }
}