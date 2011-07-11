namespace Volta.Core.Domain.Administration
{
    public interface IUserFactory
    {
        User Create(string username, string password, bool admin);
    }
}