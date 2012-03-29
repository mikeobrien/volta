using System.Linq;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Login
{
    public class IndexModel
    {
        public bool IsNewInstall { get; set; }
    }

    public class PublicGetHandler
    {
        private readonly IRepository<User> _users;

        public PublicGetHandler(IRepository<User> users)
        {
            _users = users;
        }

        public IndexModel Execute()
        {
            return new IndexModel { IsNewInstall = !_users.Any() };
        }
    }
}