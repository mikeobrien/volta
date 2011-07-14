using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public interface IUserModificationService
    {
        User Modify(Username username, User modifedUser);
    }
}