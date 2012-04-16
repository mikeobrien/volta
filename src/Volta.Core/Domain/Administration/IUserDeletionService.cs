using System;

namespace Volta.Core.Domain.Administration
{
    public interface IUserDeletionService
    {
        void Delete(Guid id);
    }
}