using Volta.Core.Domain.Administration;

namespace Volta.Web.Admin.Users
{
    public class UserPutHandler
    {
        private readonly IUserUpdateService _modificationService;

        public UserPutHandler(IUserUpdateService modificationService)
        {
            _modificationService = modificationService;
        }

        public void Execute_id(UserModel request)
        {
            _modificationService.Modify(request.id, request.username, request.email, request.administrator, request.password);
        }
    }
}