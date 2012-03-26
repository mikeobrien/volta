using NSubstitute;
using NUnit.Framework;
using Should;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web;

namespace Volta.Tests.Unit.UserInterface
{
    [TestFixture]
    public class IndexGetHandlerTests
    {
        [Test]
        public void should_return_authentication_flag()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(true);
            var handler = new IndexGetHandler(secureSession);
            var model = handler.Execute();
            model.isLoggedIn.ShouldBeTrue();
        }
    }
}