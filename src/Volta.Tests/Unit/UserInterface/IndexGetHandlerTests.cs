using NSubstitute;
using NUnit.Framework;
using Should;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Application;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web;

namespace Volta.Tests.Unit.UserInterface
{
    [TestFixture]
    public class IndexGetHandlerTests
    {
        [Test]
        public void should()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token("nbohr", true));
            secureSession.IsLoggedIn().Returns(true);
            var handler = new IndexGetHandler(secureSession, Substitute.For<ISystemInfo>());
            var model = handler.Execute();
            model.Username.ShouldEqual("nbohr");
            model.IsAdministrator.ShouldEqual(true);
        }
    }
}