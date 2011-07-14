using NSubstitute;
using NUnit.Framework;
using Should;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web.Handlers;
using Volta.Web.Handlers.Administration.Users;

namespace Volta.Tests.Unit.UserInterface.Administration
{
    [TestFixture]
    public class EditHandlerTests
    {
        private const string Username1 = "someuser";
        private const string Username2 = "andanotheruser";
        private const string Password = "password";
        private const string PasswordHash = "FFFFFFFFFF";
        private IRepository<User> _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new MemoryRepository<User>(new User { Username = Username1, Administrator = true, Password = PasswordHash });
        }

        [Test]
        public void Should_Return_Specfied_User()
        {
            var handler = new EditHandler(_userRepository, new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>()));
            var query = new EditQueryModel { Username = Username1 };
            var result = handler.Query_Username(query);
            result.Message.ShouldBeNull();
            result.Username.ShouldEqual(Username1);
            result.NewUsername.ShouldEqual(Username1);
            result.Administrator.ShouldBeTrue();
        }

        [Test]
        public void Should_Display_Message_If_User_Not_Found()
        {
            var handler = new EditHandler(_userRepository, new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>()));
            var query = new EditQueryModel { Username = "yada" };
            var result = handler.Query_Username(query);
            result.Message.MessageText.ShouldEqual(EditHandler.UserNotFoundMessage);
            result.Username.ShouldBeNull();
            result.NewUsername.ShouldBeNull();
            result.Administrator.ShouldBeFalse();
        }

        [Test]
        public void Should_Update_Existing_User()
        {
            var modificationService = Substitute.For<IUserModificationService>();
            var handler = new EditHandler(_userRepository, modificationService);
            var result = handler.Command(new EditInputModel { Username = Username1, NewUsername = Username2, Administrator = true, Password = Password });
            modificationService.Received().Modify(Arg.Is<Username>(x => x == Username1), Arg.Is<User>(x => x.Username == Username2 && x.Administrator && x.Password == Password));
            result.AssertWasRedirectedTo<QueryHandler>(x => x.Query());
        }

        [Test]
        public void Should_Return_An_Error_If_Trying_To_Update_A_User_That_Doesent_Exist()
        {
            var modificationService = Substitute.For<IUserModificationService>();
            modificationService.WhenForAnyArgs(x => x.Modify(null, null)).Do(x => {throw new UserNotFoundException();});
            var handler = new EditHandler(_userRepository, modificationService);
            var result = handler.Command(new EditInputModel());
            result.AssertWasTransferedTo(new EditOutputModel { Message = MessageModel.Error(EditHandler.UserNotFoundMessage)});
        }

        [Test]
        public void Should_Return_A_Message_If_The_New_Username_Already_Exists()
        {
            var modificationService = Substitute.For<IUserModificationService>();
            modificationService.WhenForAnyArgs(x => x.Modify(null, null)).Do(x => { throw new DuplicateUsernameException(); });
            var handler = new EditHandler(_userRepository, modificationService);
            var result = handler.Command(new EditInputModel { Username = Username1, NewUsername = Username2, Administrator = true });
            result.AssertWasTransferedTo(new EditOutputModel { Username = Username1, NewUsername = Username2, Administrator = true, 
                                                               Message = MessageModel.Error(EditHandler.DuplicateUserFoundMessage) });
        }

        [Test]
        public void Should_Return_A_Message_If_The_Username_Is_Empty()
        {
            var modificationService = Substitute.For<IUserModificationService>();
            modificationService.WhenForAnyArgs(x => x.Modify(null, null)).Do(x => { throw new EmptyUsernameException(); });
            var handler = new EditHandler(_userRepository, modificationService);
            var result = handler.Command(new EditInputModel { Username = Username1, NewUsername = Username2, Administrator = true });
            result.AssertWasTransferedTo(new EditOutputModel { Username = Username1, NewUsername = Username2, Administrator = true, 
                                                               Message = MessageModel.Error(EditHandler.EmptyUsernameMessage) });
        }
    }
}