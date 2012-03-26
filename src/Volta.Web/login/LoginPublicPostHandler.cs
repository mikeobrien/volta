using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web
{
    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginResponse
    {
        public bool success { get; set; }
    }

    public class LoginPublicPostHandler
    {
        private readonly ISecureSession<Token> _secureSession;

        public LoginPublicPostHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public LoginResponse Execute(LoginRequest request)
        {
            return new LoginResponse
            { success = _secureSession.Login(request.username, request.password) };
        }
    }
}