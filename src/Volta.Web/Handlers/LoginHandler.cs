using System;
using FubuCore;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using Volta.Core.Infrastructure;
using FubuMVC.Core.Continuations;
using Volta.Core.Infrastructure.Framework;

namespace Volta.Web.Handlers
{
    public class LoginInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RedirectUrl { get; set; }
        public string Action { get; set; }
    }

    public class LoginOutputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }

        public override bool Equals(object obj) { return this.ObjectEquals(obj); }
        public override int GetHashCode() { return this.ObjectHashCode(Username, Password, Message, RedirectUrl); }
    }

    public class LoginHandler
    {
        public const string AuthorizationErrorMessage = "You need to login to access this resource.";
        public const string AuthenticationErrorMessage = "Invalid username or password.";

        private readonly ISecureSession _secureSession;

        public LoginHandler(ISecureSession secureSession)
        {
            _secureSession = secureSession;
        }

        public LoginOutputModel Query(LoginOutputModel loginOutputModel)
        {
            if (!loginOutputModel.RedirectUrl.IsEmpty() && loginOutputModel.Message.IsEmpty())
                loginOutputModel.Message = AuthorizationErrorMessage;
            return loginOutputModel;
        }

        public FubuContinuation Command(LoginInputModel loginInputModel)
        {
            try
            {
                _secureSession.Login(loginInputModel.Username, loginInputModel.Password);
                return string.IsNullOrEmpty(loginInputModel.RedirectUrl) ? 
                    FubuContinuation.RedirectTo<DashboardHandler>(x => x.Query()) : 
                    FubuContinuation.RedirectTo(loginInputModel.RedirectUrl);
            }
            catch (Exception e)
            {
                if (e is AccessDeniedException || 
                    e is EmptyUsernameOrPasswordException)
                    return FubuContinuation.TransferTo(
                        new LoginOutputModel
                            {
                                Username = loginInputModel.Username,
                                Message = AuthenticationErrorMessage,
                                RedirectUrl = loginInputModel.RedirectUrl
                            });
                throw;
            }
        }
    }
}