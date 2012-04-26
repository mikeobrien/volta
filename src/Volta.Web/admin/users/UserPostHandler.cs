using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Admin.Users
{
    public class UserPostHandler
    {
        private readonly IRepository<User> _users;
        private readonly IUserFactory _userFactory;

        public UserPostHandler(IRepository<User> users, IUserFactory userFactory)
        {
            _users = users;
            _userFactory = userFactory;
        }

        public void Execute(UserModel request)
        {
            _users.Add(_userFactory.Create(request.username, request.password,request.email, request.administrator));
        }
    }
}