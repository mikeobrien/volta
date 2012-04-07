using AutoMapper;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.AutoMapper;
using Volta.Web.Admin.Users;

namespace Volta.Web
{
    public class MappingConventions
    {
        public static void Register()
        {
            Mapper.CreateMap<User, UserModel>().ForMember(x => x.password, x => x.Ignore());
            Mapper.CreateMap<UserModel, User>();
        }
    }
}