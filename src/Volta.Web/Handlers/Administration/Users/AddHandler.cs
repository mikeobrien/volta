using System;
using FubuMVC.Core.Continuations;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Application;

namespace Volta.Web.Handlers.Administration.Users
{
    public class AddOutputModel : ComparableModelBase
    {
        public MessageModel Message { get; set; }
        public string Username { get; set; }
        public bool Administrator { get; set; }
    }

    public class AddInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Administrator { get; set; }
    }

    public class AddHandler
    {
        public const string DuplicateUserFoundMessage = "A user with that username already exists.";
        public const string EmptyUsernameMessage = "Username is empty.";
        public const string EmptyPasswordMessage = "Password is empty.";

        private readonly IUserCreationService _creationService;

        public AddHandler(IUserCreationService creationService)
        {
            _creationService = creationService;
        }

        public AddOutputModel Query(AddOutputModel optput)
        {
            return optput;
        }

        public FubuContinuation Command(AddInputModel input)
        {
            try
            {
                _creationService.Create(input.Username, input.Password, input.Administrator);
            }
            catch (Exception e)
            {
                if (e is EmptyUsernameException ||
                    e is EmptyPasswordException ||
                    e is DuplicateUsernameException)
                {
                    var output = new AddOutputModel
                    {
                        Username = input.Username,
                        Administrator = input.Administrator
                    };
                    if (e is EmptyUsernameException) output.Message = MessageModel.Error(EmptyUsernameMessage);
                    if (e is EmptyPasswordException) output.Message = MessageModel.Error(EmptyPasswordMessage);
                    if (e is DuplicateUsernameException) output.Message = MessageModel.Error(DuplicateUserFoundMessage);
                    return FubuContinuation.TransferTo(output);
                }
                throw;
            }
            return FubuContinuation.RedirectTo<QueryHandler>(x => x.Query());
        }
    }
}