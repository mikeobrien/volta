namespace Volta.Core.Domain
{
    public interface IUserFactory
    {
        User Create(string username, string password, bool admin);
    }
}