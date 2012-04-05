using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.logout
{
    public class LogoutRequest {}

    public class PublicPostHandler
    {
        private readonly ISecureSession<Token> _secureSession;

        public PublicPostHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public void Execute(LogoutRequest request)
        {
            _secureSession.Logout();
        }
    }
}