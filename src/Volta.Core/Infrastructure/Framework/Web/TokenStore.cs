using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public class TokenStore<TToken> : ITokenStore<TToken> where TToken : class
    {
        public const string SessionKey = "TokenStore-3633A5D8-5373-11E1-958B-96A44824019B";
        private readonly ISession _sessionState;

        public TokenStore(ISession sessionState)
        {
            _sessionState = sessionState;
        }

        public TToken Get()
        {
            return (TToken)_sessionState[SessionKey];
        }

        public void Set(TToken token)
        {
            _sessionState[SessionKey] = token;
        }

        public bool Exists()
        {
            return Get() != null;
        }

        public void Clear()
        {
            Set(null);
        }
    }
}
