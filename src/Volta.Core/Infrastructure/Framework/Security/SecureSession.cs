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

        public void Login(string username, string password)
        {
            Logout();
            _tokenStore.Set(_authenticationService.Authenticate(username, password));
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