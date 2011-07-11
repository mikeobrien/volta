namespace Volta.Core.Domain.Administration
{
    public interface IUserModificationService
    {
        User Modify(string username, User modifedUser);
    }
}