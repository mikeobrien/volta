using System.Linq;
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
    public class DeleteHandlerTests
    {
        private IRepository<User> _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new MemoryRepository<User>(Enumerable.Range(1, 10).Select(x => new User { Username = x.ToString(), Administrator = (x % 2 == 0) }).ToArray());
        }

        [Test]
        public void Should_Delete_User()
        {
            var handler = new DeleteHandler(_userRepository);
            var output = handler.Query_Username(new DeleteInputModel { Username = "5"});
            _userRepository.Count().ShouldEqual(9);
            _userRepository.Any(x => x.Username == "5").ShouldBeFalse();
            output.AssertWasRedirectedTo<QueryHandler>(x => x.Query());
        }
    }
}