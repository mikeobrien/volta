using System;
using NSubstitute;
using NUnit.Framework;
using Should;
using System.Linq;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Tests.Unit.Domain.Administration
{
    [TestFixture]
    public class UserDeletionServiceTests
    {
        private const string Username1 = "someuser";
        private const string Username2 = "anotheruser";
        private IRepository<User> _userRepository;
        private static readonly Guid UserId = Guid.NewGuid();
            
        [SetUp]
        public void Setup()
        {
            _userRepository = new MemoryRepository<User>(new User { Id = UserId, Username = Username2, Administrator = true, PasswordHash = null });
        }

        [Test]
        public void should_delete_user()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(Guid.NewGuid(), Username1, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserDeletionService(_userRepository, secureSession);
            service.Delete(UserId);
            _userRepository.Count().ShouldEqual(0);
        }


        [Test]
        public void should_throw_exception_if_deleting_the_currently_logged_in_user()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(UserId, Username1, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserDeletionService(_userRepository, secureSession);
            Assert.Throws<DeleteCurrentUserException>(() => service.Delete(UserId));
            _userRepository.Count().ShouldEqual(1);
        }
    }
}