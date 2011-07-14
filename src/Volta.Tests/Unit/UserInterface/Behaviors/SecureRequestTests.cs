using System;
using System.Linq.Expressions;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using NSubstitute;
using NUnit.Framework;
using Should;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web.Behaviors;
using Volta.Web.Handlers;

namespace Volta.Tests.Unit.UserInterface.Behaviors
{
    [TestFixture]
    public class SecureRequestTests
    {
        public class LoginHandler { public string Query() { return null; } }
        public class LogoutHandler { public string Query() { return null; } }
        public class DefaultHandler { public string Query() { return null; } }

        private const string RawUrl = "/somepage/yada";
        private const string LoginUrl = "/login";
        private const string LogoutUrl = "/logout";
        private const string DefaultUrl = "/";

        [Test]
        public void Should_Return_Logged_In()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(true);
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(null, null, secureSession, null, null, null);
            request.IsLoggedIn().ShouldBeTrue();
        }

        [Test]
        public void Should_Return_Raw_Url()
        {
            var currentRequest = new CurrentRequest {RawUrl = RawUrl};
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(null, currentRequest, null, null, null, null);
            request.Url().ShouldEqual(RawUrl);
        }

        [Test]
        public void Should_Return_Login_Page_Url()
        {
            Expression<Action<LoginHandler>> property = x => x.Query();
            var urlRegistry = Substitute.For<IUrlRegistry>();
            urlRegistry.UrlFor(property).Returns(LoginUrl);
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(urlRegistry, null, null, property, null, null);
            request.LoginPageUrl().ShouldEqual(LoginUrl);
        }

        [Test]
        public void Should_Return_Default_Page_Url()
        {
            Expression<Action<DefaultHandler>> property = x => x.Query();
            var urlRegistry = Substitute.For<IUrlRegistry>();
            urlRegistry.UrlFor(property).Returns(DefaultUrl);
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(urlRegistry, null, null, null, null, property);
            request.DefaultPageUrl().ShouldEqual(DefaultUrl);
        }

        [Test]
        public void Should_Be_On_Login_Page()
        {
            var currentRequest = new CurrentRequest { Path = LoginUrl };
            Expression<Action<LoginHandler>> property = x => x.Query();
            var urlRegistry = Substitute.For<IUrlRegistry>();
            urlRegistry.UrlFor(property).Returns(LoginUrl);
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(urlRegistry, currentRequest, null, property, null, null);
            request.IsOnLoginPage().ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Be_On_Login_Page()
        {
            var currentRequest = new CurrentRequest { Path = DefaultUrl };
            Expression<Action<LoginHandler>> property = x => x.Query();
            var urlRegistry = Substitute.For<IUrlRegistry>();
            urlRegistry.UrlFor(property).Returns(LoginUrl);
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(urlRegistry, currentRequest, null, property, null, null);
            request.IsOnLoginPage().ShouldBeFalse();
        }

        [Test]
        public void Should_Be_On_Default_Page()
        {
            var currentRequest = new CurrentRequest { Path = DefaultUrl };
            Expression<Action<DefaultHandler>> property = x => x.Query();
            var urlRegistry = Substitute.For<IUrlRegistry>();
            urlRegistry.UrlFor(property).Returns(DefaultUrl);
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(urlRegistry, currentRequest, null, null, null, property);
            request.IsOnDefaultPage().ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Be_On_Default_Page()
        {
            var currentRequest = new CurrentRequest { Path = LoginUrl };
            Expression<Action<DefaultHandler>> property = x => x.Query();
            var urlRegistry = Substitute.For<IUrlRegistry>();
            urlRegistry.UrlFor(property).Returns(DefaultUrl);
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(urlRegistry, currentRequest, null, null, null, property);
            request.IsOnDefaultPage().ShouldBeFalse();
        }

        [Test]
        public void Should_Be_On_Logout_Page()
        {
            var currentRequest = new CurrentRequest { Path = LogoutUrl };
            Expression<Action<LogoutHandler>> property = x => x.Query();
            var urlRegistry = Substitute.For<IUrlRegistry>();
            urlRegistry.UrlFor(property).Returns(LogoutUrl);
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(urlRegistry, currentRequest, null, null, property, null);
            request.IsOnLogoutPage().ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Be_On_Logout_Page()
        {
            var currentRequest = new CurrentRequest { Path = DefaultUrl };
            Expression<Action<LogoutHandler>> property = x => x.Query();
            var urlRegistry = Substitute.For<IUrlRegistry>();
            urlRegistry.UrlFor(property).Returns(LogoutUrl);
            var request = new SecureRequest<LoginHandler, LogoutHandler, DefaultHandler>(urlRegistry, currentRequest, null, null, property, null);
            request.IsOnLogoutPage().ShouldBeFalse();
        }
    }
}