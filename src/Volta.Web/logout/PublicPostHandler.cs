using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Logout
{
    public class LogoutModel {}

    public class PublicPostHandler
    {
        private readonly ISecureSession<Token> _secureSession;

        public PublicPostHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public LogoutModel Execute(LogoutModel request)
        {
            _secureSession.Logout();
            return request; // Need to return something so that an empty json object is returned. If not it does weird stuff in FF.
        }
    }
}