using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.login
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
    }

    public class PublicPostHandler
    {
        private readonly ISecureSession<Token> _secureSession;

        public PublicPostHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public LoginResponse Execute(LoginRequest request)
        {
            return new LoginResponse
            { Success = _secureSession.Login(request.Username, request.Password) };
        }
    }
}