using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Admin.Users
{
    public class UsersGetRequest
    {
        public int Index { get; set; }
    }

    public class UsersGetHandler
    {
        public const int PageSize = 20;
        private readonly IRepository<User> _users;

        public UsersGetHandler(IRepository<User> users)
        {
            _users = users;
        }

        public List<UserModel> Execute(UsersGetRequest request)
        {
            return Mapper.Map<List<UserModel>>(_users.OrderBy(x => x.Username).Page(request.Index, PageSize).ToList());
        }
    }
}