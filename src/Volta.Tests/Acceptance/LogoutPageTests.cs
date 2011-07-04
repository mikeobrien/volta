using NUnit.Framework;
using Should;
using Volta.Tests.Acceptance.Common;

namespace Volta.Tests.Acceptance
{
    public class LogoutPageTests : WebPageTestBase<LoginPage>
    {
        [Test]
        public void Should_Logout_And_Redirect_To_Login_Page()
        {
            Page.UsernameTextField.TypeText(Constants.TestUsername);
            Page.PasswordTextField.TypeText(Constants.TestPassword);
            Page.Submit().NavigateTo<LogoutPage>().
                          SwitchTo<LoginPage>().
                          IsOnPage().ShouldBeTrue();
        }
    }
}
