using System.Linq;
using FubuMVC.Core.Continuations;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Application;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Handlers.Administration.Users
{
    public class EditQueryModel
    {
        public string Username { get; set; }
    }

    public class EditOutputModel : ComparableModelBase
    {
        public MessageModel Message { get; set; }
        public string Username { get; set; }
        public string NewUsername { get; set; }
        public bool IsAdministrator { get; set; }
    }

    public class EditInputModel
    {
        public string Username { get; set; }
        public string NewUsername { get; set; }
        public string Password { get; set; }
        public bool IsAdministrator { get; set; }
    }

    public class EditHandler
    {
        public const string UserNotFoundMessage = "User not found.";
        public const string DuplicateUserFoundMessage = "A user with that username already exists.";

        private readonly IRepository<User> _userRepository;
        private readonly IUserModificationService _userModificationService;

        public EditHandler(IRepository<User> userRepository, IUserModificationService userModificationService)
        {
            _userRepository = userRepository;
            _userModificationService = userModificationService;
        }

        public EditOutputModel Query(EditOutputModel output)
        {
            return output;
        }

        public EditOutputModel Query_Username(EditQueryModel query)
        {
            var user = _userRepository.FirstOrDefault(x => x.Username == query.Username);
            return user != null 
                ? new EditOutputModel
                    {
                        Username = user.Username,
                        NewUsername = user.Username,
                        IsAdministrator = user.Administrator
                    }
                : new EditOutputModel { Message = MessageModel.Error(UserNotFoundMessage) };
        }
        
        public FubuContinuation Command(EditInputModel input)
        {
            try
            {
                _userModificationService.Modify(
                    input.Username, 
                    new User
                        {
                            Username = input.NewUsername,
                            Password = input.Password,
                            Administrator = input.IsAdministrator
                        });
            }
            catch (UserNotFoundException)
            {
                return FubuContinuation.TransferTo(
                    new EditOutputModel { Message = MessageModel.Error(UserNotFoundMessage) });
            }
            catch (DuplicateUsernameException)
            {
                return FubuContinuation.TransferTo(new EditOutputModel { 
                    Username = input.Username,
                    NewUsername = input.NewUsername,
                    IsAdministrator = input.IsAdministrator,
                    Message = MessageModel.Information(DuplicateUserFoundMessage) });
            }
            return FubuContinuation.RedirectTo<QueryHandler>(x => x.Query());
        }
    }
}