using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Behaviors
{
    public class AuthorizationBehavior : IActionBehavior
    {
        public const string LoginUrl = "/login/";

        private readonly IOutputWriter _writer;
        private readonly IActionBehavior _actionBehavior;
        private readonly ISecureSession<Token> _secureSession;

        public AuthorizationBehavior(
            IOutputWriter writer, 
            IActionBehavior actionBehavior, 
            ISecureSession<Token> secureSession)
        {
            _writer = writer;
            _actionBehavior = actionBehavior;
            _secureSession = secureSession;
        }

        public void Invoke()
        {
            if (!_secureSession.IsLoggedIn()) _writer.RedirectToUrl(LoginUrl);
            else _actionBehavior.Invoke();
        }

        public void InvokePartial()
        {
            _actionBehavior.InvokePartial();
        }
    }
}