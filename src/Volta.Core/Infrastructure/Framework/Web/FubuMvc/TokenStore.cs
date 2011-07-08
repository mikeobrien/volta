using FubuMVC.Core.Runtime;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Infrastructure.Framework.Web.FubuMvc
{
    public class FubuTokenStore<TToken> : ITokenStore<TToken> where TToken : class
    {
        private readonly ISessionState _sessionState;

        public FubuTokenStore(ISessionState sessionState)
        {
            _sessionState = sessionState;
        }

        public TToken Get()
        {
            return _sessionState.Get<TToken>();
        }

        public void Set(TToken token)
        {
            _sessionState.Set(token);
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
