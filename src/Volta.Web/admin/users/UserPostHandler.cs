using Volta.Core.Application.Security;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Admin.Users
{
    public class UserPostHandler
    {
        private readonly IRepository<User> _users;
        private readonly IUserFactory _userFactory;
        private readonly ISecureSession<Token> _secureSession;

        public UserPostHandler(IRepository<User> users, IUserFactory userFactory, ISecureSession<Token> secureSession)
        {
            _users = users;
            _userFactory = userFactory;
            _secureSession = secureSession;
        }

        public void Execute(UserModel request)
        {
            _userFactory.Create(
                request.username, 
                request.password,
                request.email, 
                request.administrator, 
                _secureSession.GetCurrentToken().Username);
        }
    }
}