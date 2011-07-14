using NSubstitute;
using NUnit.Framework;
using Volta.Core.Domain.Administration;
using Volta.Web.Handlers;
using Volta.Web.Handlers.Administration.Users;

namespace Volta.Tests.Unit.UserInterface.Administration
{
    [TestFixture]
    public class AddHandlerTests
    {
        private const string Username = "someuser";

        [Test]
        public void Should_Add_User()
        {
            var creationService = Substitute.For<IUserCreationService>();
            var handler = new AddHandler(creationService);
            var result = handler.Command(new AddInputModel { Username = Username, Administrator = true});
            creationService.Received().Create(Username, null, true);
            result.AssertWasRedirectedTo<QueryHandler>(x => x.Query());
        }

        [Test]
        public void Should_Return_A_Message_If_The_Username_Already_Exists()
        {
            var creationService = Substitute.For<IUserCreationService>();
            creationService.WhenForAnyArgs(x => x.Create(null, null, false)).Do(x => { throw new DuplicateUsernameException(); });
            var handler = new AddHandler(creationService);
            var result = handler.Command(new AddInputModel { Username = Username, Administrator = true });
            result.AssertWasTransferedTo(new AddOutputModel { Username = Username, Administrator = true, 
                                                               Message = MessageModel.Error(AddHandler.DuplicateUserFoundMessage) });
        }

        [Test]
        public void Should_Return_A_Message_If_The_Username_Is_Empty()
        {
            var creationService = Substitute.For<IUserCreationService>();
            creationService.WhenForAnyArgs(x => x.Create(null, null, false)).Do(x => { throw new EmptyUsernameException(); });
            var handler = new AddHandler(creationService);
            var result = handler.Command(new AddInputModel { Username = Username, Administrator = true });
            result.AssertWasTransferedTo(new AddOutputModel { Username = Username, Administrator = true,
                                                              Message = MessageModel.Error(AddHandler.EmptyUsernameMessage) });
        }

        [Test]
        public void Should_Return_A_Message_If_The_Password_Is_Empty()
        {
            var creationService = Substitute.For<IUserCreationService>();
            creationService.WhenForAnyArgs(x => x.Create(null, null, false)).Do(x => { throw new EmptyPasswordException(); });
            var handler = new AddHandler(creationService);
            var result = handler.Command(new AddInputModel { Username = Username, Administrator = true });
            result.AssertWasTransferedTo(new AddOutputModel { Username = Username, Administrator = true,
                                                              Message = MessageModel.Error(AddHandler.EmptyPasswordMessage) });
        }
    }
}