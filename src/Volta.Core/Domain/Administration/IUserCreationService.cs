namespace Volta.Core.Domain.Administration
{
    public interface IUserCreationService
    {
        User Create(string username, string password, bool isAdmin);
    }
}