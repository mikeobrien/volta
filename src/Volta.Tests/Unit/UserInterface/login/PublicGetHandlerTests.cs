using NUnit.Framework;
using Should;
using Volta.Core.Domain.Administration;
using Volta.Web.Login;

namespace Volta.Tests.Unit.UserInterface.Login
{
    [TestFixture]
    public class PublicGetHandlerTests
    {
        [Test]
        public void should_indicate_new_install_when_there_are_no_users()
        {
            var userRepository = new MemoryRepository<User>();
            var handler = new PublicGetHandler(userRepository);
            var model = handler.Execute();
            model.IsNewInstall.ShouldBeTrue();
        }

        [Test]
        public void should_not_indicate_new_install_flag_when_there_are_users()
        {
            var userRepository = new MemoryRepository<User>(new User());
            var handler = new PublicGetHandler(userRepository);
            var model = handler.Execute();
            model.IsNewInstall.ShouldBeFalse();
        }
    }
}