namespace Volta.Core.Infrastructure.Framework.Security
{
    public interface IAuthenticationService<out TToken> where TToken : class
    {
        TToken Authenticate(Username username, string password);
    }
}