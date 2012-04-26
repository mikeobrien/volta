using Volta.Core.Domain.Administration;

namespace Volta.Web.Admin.Users
{
    public class UserDeleteHandler
    {
        private readonly IUserDeleteService _deleteService;

        public UserDeleteHandler(IUserDeleteService deleteService)
        {
            _deleteService = deleteService;
        }

        public void Execute_id(UserModel request)
        {
            _deleteService.Delete(request.id);
        }
    }
}