using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public interface IUserCreationService
    {
        User Create(Username username, string password, string email, bool isAdministrator);
    }
}