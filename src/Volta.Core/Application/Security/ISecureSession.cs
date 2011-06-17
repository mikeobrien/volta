namespace Volta.Core.Application.Security
{
    public interface ISecureSession
    {
        void Login(string username, string password);
        void Logout();
        Token GetCurrentToken();
        bool IsLoggedIn();
    }
}