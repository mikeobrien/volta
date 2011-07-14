using System;
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
        public bool Administrator { get; set; }
    }

    public class EditInputModel
    {
        public string Username { get; set; }
        public string NewUsername { get; set; }
        public string Password { get; set; }
        public bool Administrator { get; set; }
    }

    public class EditHandler
    {
        public const string UserNotFoundMessage = "User not found.";
        public const string DuplicateUserFoundMessage = "A user with that username already exists.";
        public const string EmptyUsernameMessage = "Username is empty.";

        private readonly IRepository<User> _users;
        private readonly IUserModificationService _modificationService;

        public EditHandler(IRepository<User> users, IUserModificationService modificationService)
        {
            _users = users;
            _modificationService = modificationService;
        }

        public EditOutputModel Query(EditOutputModel output)
        {
            return output;
        }

        public EditOutputModel Query_Username(EditQueryModel query)
        {
            var user = _users.FirstOrDefault(x => x.Username == query.Username);
            return user != null 
                ? new EditOutputModel
                    {
                        Username = user.Username,
                        NewUsername = user.Username,
                        Administrator = user.Administrator
                    }
                : new EditOutputModel { Message = MessageModel.Error(UserNotFoundMessage) };
        }
        
        public FubuContinuation Command(EditInputModel input)
        {
            try
            {
                _modificationService.Modify(
                    input.Username,
                    new User
                        {
                            Username = input.NewUsername,
                            Password = input.Password,
                            Administrator = input.Administrator
                        });
            }
            catch (Exception e)
            {
                if (e is EmptyUsernameException ||
                    e is UserNotFoundException ||
                    e is DuplicateUsernameException)
                {
                    var output = new EditOutputModel
                        {
                            Username = input.Username,
                            NewUsername = input.NewUsername,
                            Administrator = input.Administrator
                        };
                    if (e is EmptyUsernameException) output.Message = MessageModel.Error(EmptyUsernameMessage);
                    if (e is UserNotFoundException) output.Message = MessageModel.Error(UserNotFoundMessage);
                    if (e is DuplicateUsernameException) output.Message = MessageModel.Error(DuplicateUserFoundMessage);
                    return FubuContinuation.TransferTo(output);
                }
                throw;
            }
            return FubuContinuation.RedirectTo<QueryHandler>(x => x.Query());
        }
    }
}