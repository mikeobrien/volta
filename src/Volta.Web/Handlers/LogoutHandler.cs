using Volta.Core.Application.Security;
using FubuMVC.Core.Continuations;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Handlers
{
    public class LogoutHandler
    {
        private readonly ISecureSession<Token> _secureSession;

        public LogoutHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public FubuContinuation Query()
        {
            _secureSession.Logout();
            return FubuContinuation.RedirectTo<LoginHandler>(x => x.Query(null));
        }
    }
}