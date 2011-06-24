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
        private readonly IOutputWriter _writer;
        private readonly IActionBehavior _actionBehavior;
        private readonly ISecureSession _secureSession;

        public AuthenticationBehavior(IUrlRegistry registry, IOutputWriter writer, IActionBehavior actionBehavior, ISecureSession secureSession)
        {
            _registry = registry;
            _writer = writer;
            _actionBehavior = actionBehavior;
            _secureSession = secureSession;
        }

        public void Invoke()
        {
            if (_secureSession.IsLoggedIn())_actionBehavior.Invoke();
            else _writer.RedirectToUrl(_registry.UrlFor<LoginHandler>(x => x.Query(null)));
        }

        public void InvokePartial()
        {
            if (_secureSession.IsLoggedIn()) _actionBehavior.InvokePartial();
        }
    }
}