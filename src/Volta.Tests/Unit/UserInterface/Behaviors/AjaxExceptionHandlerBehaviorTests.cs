using System;
using System.Net;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using NSubstitute;
using NUnit.Framework;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Web;
using Volta.Web.Behaviors;

namespace Volta.Tests.Unit.UserInterface.Behaviors
{
    [TestFixture]
    public class AjaxExceptionHandlerBehaviorTests
    {
        [Test]
        public void should_not_do_anything_if_there_is_not_an_exception()
        {
            var outputWriter = Substitute.For<IOutputWriter>();
            var innerBehavior = Substitute.For<IActionBehavior>();
            var logger = Substitute.For<ILogger>();
            var webServer = Substitute.For<IWebServer>();

            var exceptionHandlerBehavior = new AjaxExceptionHandlerBehavior(innerBehavior, outputWriter, logger, webServer);

            exceptionHandlerBehavior.Invoke();

            innerBehavior.Received().Invoke();
            webServer.DidNotReceiveWithAnyArgs().IgnoreErrorStatus = true;
            outputWriter.DidNotReceiveWithAnyArgs().WriteResponseCode(0);
            outputWriter.DidNotReceiveWithAnyArgs().WriteResponseCode(HttpStatusCode.OK);
            logger.DidNotReceiveWithAnyArgs().Write(null);
        }

        [Test]
        public void should_log_and_set_status_to_500_when_there_is_an_unhandled_exception()
        {
            var outputWriter = Substitute.For<IOutputWriter>();
            var innerBehavior = Substitute.For<IActionBehavior>();
            var logger = Substitute.For<ILogger>();
            var webServer = Substitute.For<IWebServer>();
            var exception = new Exception("bad things happening");

            innerBehavior.When(x => x.Invoke()).Do(x => { throw exception; });

            var exceptionHandlerBehavior = new AjaxExceptionHandlerBehavior(innerBehavior, outputWriter, logger, webServer);

            exceptionHandlerBehavior.Invoke();

            webServer.Received().IgnoreErrorStatus = true;
            innerBehavior.Received().Invoke();
            outputWriter.Received().WriteResponseCode(HttpStatusCode.InternalServerError, "A system error has occured.");
            logger.Received().Write(exception);
        }

        [Test]
        public void should_not_log_and_set_status_to_403_when_there_is_an_authorization_exception()
        {
            var outputWriter = Substitute.For<IOutputWriter>();
            var innerBehavior = Substitute.For<IActionBehavior>();
            var logger = Substitute.For<ILogger>();
            var webServer = Substitute.For<IWebServer>();

            innerBehavior.When(x => x.Invoke()).Do(x => { throw new AuthorizationException(); });

            var exceptionHandlerBehavior = new AjaxExceptionHandlerBehavior(innerBehavior, outputWriter, logger, webServer);

            exceptionHandlerBehavior.Invoke();

            webServer.Received().IgnoreErrorStatus = true;
            innerBehavior.Received().Invoke();
            outputWriter.Received().WriteResponseCode(HttpStatusCode.Forbidden, "You are not authorized to perform this action.");
            logger.DidNotReceiveWithAnyArgs().Write(null);
        }
    }
}