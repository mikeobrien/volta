using NUnit.Framework;
using Should;
using Volta.Tests.Acceptance.Common;
using Volta.Web.Handlers;

namespace Volta.Tests.Acceptance
{
    public class LoginPageTests : WebPageTestBase<LoginPage>
    {
        public LoginPageTests() : base(true) {}

        [Test]
        public void Should_Display_Login_Page()
        {
            Page.IsOnPage().ShouldBeTrue();
            Page.HasFormLabelAndInput(x => x.Username).ShouldBeTrue();
            Page.HasFormLabelAndInput(x => x.Password).ShouldBeTrue();
            Page.SubmitButton.Exists.ShouldBeTrue();
        }

        [Test]
        public void Should_Login_With_Valid_Credentials()
        {
            Page.UsernameTextField.TypeText("spin");
            Page.PasswordTextField.TypeText("onehalf");
            Page.Submit().SwitchTo<DashboardPage>().IsOnPage().ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Login_With_Invalid_Credentials()
        {
            Page.UsernameTextField.TypeText("yada");
            Page.PasswordTextField.TypeText("yada");
            Page.Submit();
            Page.IsOnPage().ShouldBeTrue();
            Page.HasMessage.ShouldBeTrue();
            Page.MessageText.ShouldEqual(LoginHandler.AuthenticationErrorMessage);
        }
         
        [Test]
        public void Should_Be_Redirected_To_The_Login_Page_When_Not_Logged_In_And_Accessing_Default_Page()
        {
            Page.NavigateTo<DashboardPage>();
            Page.IsOnPage().ShouldBeTrue();
            Page.HasMessage.ShouldBeFalse();
        }

        [Test]
        public void Should_Be_Redirected_To_The_Login_Page_When_Not_Logged_In_And_Accessing_Non_Default_Page()
        {
            Page.NavigateTo<MyAccountPage>();
            Page.IsOnPage().ShouldBeTrue();
            Page.HasMessage.ShouldBeTrue();
            Page.MessageText.ShouldEqual(LoginHandler.AuthorizationErrorMessage);
        }

        [Test]
        public void Should_Be_Redirected_To_The_Login_Page_When_Not_Logged_In_And_Accessing_Non_Default_Page_Then_Redirect_Back_On_Login()
        {
            Page.NavigateTo<MyAccountPage>();
            Page.UsernameTextField.TypeText("spin");
            Page.PasswordTextField.TypeText("onehalf");
            Page.Submit().SwitchTo<MyAccountPage>().IsOnPage().ShouldBeTrue();
        }
    }
}
