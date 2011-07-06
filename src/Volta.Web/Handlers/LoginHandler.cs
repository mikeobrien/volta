using System;
using FubuCore;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using FubuMVC.Core.Continuations;
using Volta.Core.Infrastructure.Framework;
using Volta.Core.Infrastructure.Framework.Security;

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

        private readonly ISecureSession<Token> _secureSession;

        public LoginHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public LoginOutputModel Query(LoginOutputModel input)
        {
            if (!input.RedirectUrl.IsEmpty() && input.Message.IsEmpty())
                input.Message = AuthorizationErrorMessage;
            return input;
        }

        public FubuContinuation Command(LoginInputModel input)
        {
            try
            {
                _secureSession.Login(input.Username, input.Password);
                return string.IsNullOrEmpty(input.RedirectUrl) ? 
                    FubuContinuation.RedirectTo<DashboardHandler>(x => x.Query()) : 
                    FubuContinuation.RedirectTo(input.RedirectUrl);
            }
            catch (Exception e)
            {
                if (e is AuthenticationService.AccessDeniedException || 
                    e is EmptyUsernameOrPasswordException)
                    return FubuContinuation.TransferTo(
                        new LoginOutputModel
                            {
                                Username = input.Username,
                                Message = AuthenticationErrorMessage,
                                RedirectUrl = input.RedirectUrl
                            });
                throw;
            }
        }
    }
}