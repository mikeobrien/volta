using System;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using Volta.Core.Infrastructure;
using Volta.Web.Controllers.Dashboard;
using FubuMVC.Core.Continuations;

namespace Volta.Web.Controllers.Login
{
    public class LoginController
    {
        private readonly ISecureSession _secureSession;

        public LoginController(ISecureSession secureSession)
        {
            _secureSession = secureSession;
        }

        public LoginViewModel get_Login(LoginViewModel loginViewModel)
        {
            return loginViewModel;
        }

        public FubuContinuation post_Login(LoginInputModel loginInputModel)
        {
            try
            {
                _secureSession.Login(loginInputModel.Username, loginInputModel.Password);
                return FubuContinuation.RedirectTo<DashboardController>(x => x.Index());
            }
            catch (Exception e)
            {
                if (e is AccessDeniedException || 
                    e is EmptyUsernameOrPasswordException)
                    return FubuContinuation.TransferTo(
                        new LoginViewModel
                            {
                                Username = loginInputModel.Username, 
                                AccessDenied = true
                            });
                throw;
            }
        }
    }

    public class LoginInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool AccessDenied { get; set; }

        public override bool Equals(object obj) { return this.ObjectEquals(obj); }
        public override int GetHashCode() { return this.ObjectHashCode(Username, Password, AccessDenied); }
    }
}