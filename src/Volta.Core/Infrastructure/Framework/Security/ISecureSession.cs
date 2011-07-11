namespace Volta.Core.Infrastructure.Framework.Security
{
    public interface ISecureSession<TToken> where TToken : class
    {
        void Login(string username, string password);
        void Login(TToken token);
        void Logout();
        TToken GetCurrentToken();
        bool IsLoggedIn();
    }
}