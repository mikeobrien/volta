using Volta.Core.Application.Security;
using FubuMVC.Core.Continuations;

namespace Volta.Web.Handlers
{
    public class LogoutHandler
    {
        private readonly ISecureSession _secureSession;

        public LogoutHandler(ISecureSession secureSession)
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