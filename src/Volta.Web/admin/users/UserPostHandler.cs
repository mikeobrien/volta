using Volta.Core.Domain.Administration;

namespace Volta.Web.Admin.Users
{
    public class UserPostHandler
    {
        private readonly IUserCreationService _creationService;

        public UserPostHandler(IUserCreationService creationService)
        {
            _creationService = creationService;
        }

        public void Execute(UserModel request)
        {
            _creationService.Create(request.username, request.password,request.email, request.administrator);
        }
    }
}