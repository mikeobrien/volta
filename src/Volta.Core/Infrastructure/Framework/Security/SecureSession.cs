namespace Volta.Core.Infrastructure.Framework.Security
{
    public class SecureSession<TToken> : ISecureSession<TToken> where TToken : class
    {
        private readonly IAuthenticationService<TToken> _authenticationService;
        private readonly ITokenStore<TToken> _tokenStore;

        public SecureSession(IAuthenticationService<TToken> authenticationService, ITokenStore<TToken> tokenStore)
        {
            _authenticationService = authenticationService;
            _tokenStore = tokenStore;
        }

        public bool Login(Username username, string password)
        {
            var token = _authenticationService.Authenticate(username, password);
            if (token == null) return false;
            Login(token);
            return true;
        }

        public void Login(TToken token)
        {
            Logout();
            _tokenStore.Set(token);
        }

        public void Logout()
        {
            _tokenStore.Clear();
        }

        public TToken GetCurrentToken()
        {
            return _tokenStore.Get();
        }

        public bool IsLoggedIn()
        {
            return _tokenStore.Exists();
        }
    }
}