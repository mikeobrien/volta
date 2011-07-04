using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Tests.Unit
{
    public class MemoryTokenStore<TToken> : ITokenStore<TToken> where TToken : class
    {
        private TToken _token;

        public TToken Get()
        {
            return _token;
        }

        public void Set(TToken value)
        {
            _token = value;
        }

        public bool Exists()
        {
            return _token != null;
        }

        public void Clear()
        {
            _token = null;
        }
    }
}