using Volta.Core.Domain.Administration;

namespace Volta.Web.Admin.Users
{
    public class UserDeleteHandler
    {
        private readonly IUserDeletionService _deletionService;

        public UserDeleteHandler(IUserDeletionService deletionService)
        {
            _deletionService = deletionService;
        }

        public void Execute_id(UserModel request)
        {
            _deletionService.Delete(request.id);
        }
    }
}