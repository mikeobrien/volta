using System;
using System.Linq.Expressions;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using NSubstitute;
using NUnit.Framework;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web.Behaviors;
using Volta.Web.Handlers;

namespace Volta.Tests.Unit.UserInterface.Behaviors
{
    [TestFixture]
    public class AuthorizationBehaviorTests
    {
        private IUrlRegistry _urlRegistry;

        private const string SomeUrlPath = "/somepage/yada";
        private const string SomeUrlWithQueryString = SomeUrlPath + "?oh=hai";
        private const string LoginUrl = "/login";
        private const string LogoutUrl = "/logout";
        private const string DefaultUrl = "/";

        [SetUp]
        public void Setup()
        {
            _urlRegistry = Substitute.For<IUrlRegistry>();
            _urlRegistry.UrlFor(Arg.Any<Expression<Action<LoginHandler>>>()).Returns(LoginUrl);
            _urlRegistry.UrlFor(Arg.Any<Expression<Action<LogoutHandler>>>()).Returns(LogoutUrl);
            _urlRegistry.UrlFor(Arg.Any<Expression<Action<DashboardHandler>>>()).Returns(DefaultUrl);
        }

        [Test]
        public void When_Logged_In_And_Not_On_The_Login_Page_Should_Continue()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(true);
            var currentRequest = new CurrentRequest { Path = SomeUrlPath };
            var actionBehavior = Substitute.For<IActionBehavior>();
            var behavior = new AuthorizationBehavior(_urlRegistry, currentRequest, null, actionBehavior, secureSession);
            behavior.Invoke();
            actionBehavior.Received().Invoke();
        }

        [Test]
        public void When_Logged_In_And_On_The_Login_Page_Should_Redirect_To_The_Default_Page()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(true);
            var outputWriter = Substitute.For<IOutputWriter>();
            var currentRequest = new CurrentRequest { Path = LoginUrl };
            var actionBehavior = Substitute.For<IActionBehavior>();
            var behavior = new AuthorizationBehavior(_urlRegistry, currentRequest, outputWriter, actionBehavior, secureSession);
            behavior.Invoke();
            actionBehavior.DidNotReceive().Invoke();
            outputWriter.Received().RedirectToUrl(DefaultUrl);
        }

        [Test]
        public void When_Not_Logged_In_And_On_The_Login_Page_Should_Continue()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(false);
            var currentRequest = new CurrentRequest { Path = LoginUrl };
            var actionBehavior = Substitute.For<IActionBehavior>();
            var behavior = new AuthorizationBehavior(_urlRegistry, currentRequest, null, actionBehavior, secureSession);
            behavior.Invoke();
            actionBehavior.Received().Invoke();
        }

        [Test]
        public void When_Not_Logged_In_And_On_The_Default_Page_Should_Redirect_To_The_Login_Page()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(false);
            var outputWriter = Substitute.For<IOutputWriter>();
            var currentRequest = new CurrentRequest { Path = DefaultUrl };
            var actionBehavior = Substitute.For<IActionBehavior>();
            var behavior = new AuthorizationBehavior(_urlRegistry, currentRequest, outputWriter, actionBehavior, secureSession);
            behavior.Invoke();
            actionBehavior.DidNotReceive().Invoke();
            outputWriter.Received().RedirectToUrl(LoginUrl);
        }

        [Test]
        public void When_Not_Logged_In_And_On_The_Logout_Page_Should_Redirect_To_The_Login_Page()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(false);
            var outputWriter = Substitute.For<IOutputWriter>();
            var currentRequest = new CurrentRequest { Path = LogoutUrl };
            var actionBehavior = Substitute.For<IActionBehavior>();
            var behavior = new AuthorizationBehavior(_urlRegistry, currentRequest, outputWriter, actionBehavior, secureSession);
            behavior.Invoke();
            actionBehavior.DidNotReceive().Invoke();
            outputWriter.Received().RedirectToUrl(LoginUrl);
        }

        [Test]
        public void When_Not_Logged_In_And_Not_On_The_Default_Or_Login_Page_Should_Redirect_To_The_Login_Page_With_A_Redirect_Url()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(false);
            var outputWriter = Substitute.For<IOutputWriter>();
            var currentRequest = new CurrentRequest { Path = SomeUrlPath, RawUrl = SomeUrlWithQueryString };
            var actionBehavior = Substitute.For<IActionBehavior>();
            var behavior = new AuthorizationBehavior(_urlRegistry, currentRequest, outputWriter, actionBehavior, secureSession);
            behavior.Invoke();
            actionBehavior.DidNotReceive().Invoke();
            outputWriter.Received().RedirectToUrl(LoginUrl + "?RedirectUrl=" + SomeUrlWithQueryString.UrlEncode());
        }
    }
}