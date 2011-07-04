using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using NSubstitute;
using NUnit.Framework;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Web;
using Volta.Web.Behaviors;

namespace Volta.Tests.Unit.UserInterface.Behaviors
{
    [TestFixture]
    public class ExceptionHandlerBehaviorTests
    {
        private IOutputWriter _writer;
        private CurrentRequest _request;
        private IActionBehavior _behavior;
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _writer = Substitute.For<IOutputWriter>();
            _request = new CurrentRequest { RawUrl = "http://www.google.com" };
            _behavior = Substitute.For<IActionBehavior>();
            _logger = Substitute.For<ILogger>();
        }

        [Test]
        public void Should_Log_Exception()
        {
            var exception = new Exception("Bad");
            _behavior.When(x => x.Invoke()).Do(x => { throw exception; });
            var exceptionHandler = new ExceptionHandlerBehavior(_writer, _request, _behavior, _logger);
            exceptionHandler.Invoke();
            _logger.Received().Write(_request.RawUrl, exception);
        }

        [Test]
        public void Should_Return_Error_Page()
        {
            _behavior.When(x => x.Invoke()).Do(x => { throw new Exception("Bad"); });
            const string errorPage = "<html></html>";
            var exceptionHandler = new ExceptionHandlerBehavior(_writer, _request, _behavior, _logger);
            exceptionHandler.Invoke();
            _writer.Received().RedirectToUrl("/content/error.htm");
        }

        [Test]
        public void Should_Do_Nothing_When_There_Is_No_Error()
        {
            var exceptionHandler = new ExceptionHandlerBehavior(_writer, _request, _behavior, _logger);
            exceptionHandler.Invoke();
            _behavior.Received().Invoke();
            _logger.DidNotReceiveWithAnyArgs().Write(null, null);
            _writer.DidNotReceiveWithAnyArgs().Write(null, null);
        }

        [Test]
        public void Should_Not_Catch_Partial_Exception()
        {
            _behavior.When(x => x.InvokePartial()).Do(x => { throw new Exception("Bad"); });
            var exceptionHandler = new ExceptionHandlerBehavior(_writer, _request, _behavior, _logger);
            Assert.Throws<Exception>(exceptionHandler.InvokePartial);
        }
    }
}
