using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public interface IUserFactory
    {
        User Create(Username username, string password, string email, bool admin, string createdBy);
    }
}