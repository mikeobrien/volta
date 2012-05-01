using System;
using AutoMapper;
using Volta.Core.Domain;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Admin.Users
{
    public class UsersGetHandler
    {
        private readonly IRepository<User> _users;

        public UsersGetHandler(IRepository<User> users)
        {
            _users = users;
        }

        public UserModel Execute_id(UserModel request)
        {
            var user = _users.Get(request.id);
            if (user == null) throw new NotFoundException("User");
            return Mapper.Map<UserModel>(user);
        }
    }
}