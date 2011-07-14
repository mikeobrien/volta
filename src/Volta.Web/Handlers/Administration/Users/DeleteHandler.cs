using FubuMVC.Core.Continuations;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Handlers.Administration.Users
{
    public class DeleteInputModel
    {
        public string Username { get; set; }
    }

    public class DeleteHandler
    {
        private readonly IRepository<User> _users;

        public DeleteHandler(IRepository<User> users)
        {
            _users = users;
        }
 
        public FubuContinuation Query_Username(DeleteInputModel input)
        {
            _users.Delete(x => x.Username == input.Username);
            return FubuContinuation.RedirectTo<QueryHandler>(x => x.Query());
        }
    }
}