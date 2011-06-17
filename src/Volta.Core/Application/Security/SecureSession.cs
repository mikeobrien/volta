using FubuMVC.Core.Runtime;

namespace Volta.Core.Application.Security
{
    public class SecureSession : ISecureSession
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionState _session;

        public SecureSession(IAuthenticationService authenticationService, ISessionState session)
        {
            _authenticationService = authenticationService;
            _session = session;
        }

        public void Login(string username, string password)
        {
            Logout();
            var user = _authenticationService.Authenticate(username, password);
            _session.Set(new Token(user.Username, user.IsAdmin));
        }

        public void Logout()
        {
            _session.Set<Token>(null);
        }

        public Token GetCurrentToken()
        {
            return _session.Get<Token>();
        }

        public bool IsLoggedIn()
        {
            return _session.Get<Token>() != null;
        }
    }
}