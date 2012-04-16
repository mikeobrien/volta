using System;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public interface IUserModificationService
    {
        User Modify(Guid id, Username username, string email, bool administrator, string password = null);
    }
}