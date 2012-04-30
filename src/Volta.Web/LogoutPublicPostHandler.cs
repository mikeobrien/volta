using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web
{
    public class LogoutModel {}

    public class PublicPostHandler
    {
        private readonly ISecureSession<Token> _secureSession;

        public PublicPostHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public void ExecuteLogout(LogoutModel request)
        {
            _secureSession.Logout();
        }
    }
}