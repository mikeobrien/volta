using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Admin.Users
{
    public class UserDeleteHandler
    {
        public const int PageSize = 20;
        private readonly IRepository<User> _users;

        public UserDeleteHandler(IRepository<User> users)
        {
            _users = users;
        }

        public void Execute_id(UserModel request)
        {
            _users.Delete(request.id);
        }
    }
}