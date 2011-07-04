namespace Volta.Core.Infrastructure.Framework.Security
{
    public interface ITokenStore<TToken> where TToken : class
    {
        TToken Get();
        void Set(TToken token);
        bool Exists();
        void Clear();
    }
}
