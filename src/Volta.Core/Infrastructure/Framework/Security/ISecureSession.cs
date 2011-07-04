namespace Volta.Core.Infrastructure.Framework.Security
{
    public interface ISecureSession<out TToken> where TToken : class
    {
        void Login(string username, string password);
        void Logout();
        TToken GetCurrentToken();
        bool IsLoggedIn();
    }
}