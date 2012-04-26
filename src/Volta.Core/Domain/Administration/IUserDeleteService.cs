using System;

namespace Volta.Core.Domain.Administration
{
    public interface IUserDeleteService
    {
        void Delete(Guid id);
    }
}