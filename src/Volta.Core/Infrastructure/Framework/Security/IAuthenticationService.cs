namespace Volta.Core.Infrastructure.Framework.Security
{
    public interface IAuthenticationService<out TToken> where TToken : class
    {
        TToken Authenticate(string username, string password);
    }
}