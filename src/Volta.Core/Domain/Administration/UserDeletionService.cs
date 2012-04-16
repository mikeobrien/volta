using System;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public class DeleteCurrentUserException : ValidationException 
        { public DeleteCurrentUserException() : base("Cannot delete the currently logged in user.") {}}

    public class UserDeletionService : IUserDeletionService
    {
        private readonly IRepository<User> _users;
        private readonly ISecureSession<Token> _secureSession;

        public UserDeletionService(IRepository<User> users, ISecureSession<Token> secureSession)
        {
            _users = users;
            _secureSession = secureSession;
        }

        public void Delete(Guid id)
        {
            if (_secureSession.IsLoggedIn() && _secureSession.GetCurrentToken().UserId == id)
                throw new DeleteCurrentUserException();
            _users.Delete(id);
        }
    }
}