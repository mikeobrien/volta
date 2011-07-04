using System;
using FubuCore;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
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

        public LoginOutputModel Query(LoginOutputModel model)
        {
            if (!model.RedirectUrl.IsEmpty() && model.Message.IsEmpty())
                model.Message = AuthorizationErrorMessage;
            return model;
        }

        public FubuContinuation Command(LoginInputModel model)
        {
            try
            {
                _secureSession.Login(model.Username, model.Password);
                return string.IsNullOrEmpty(model.RedirectUrl) ? 
                    FubuContinuation.RedirectTo<DashboardHandler>(x => x.Query()) : 
                    FubuContinuation.RedirectTo(model.RedirectUrl);
            }
            catch (Exception e)
            {
                if (e is AccessDeniedException || 
                    e is EmptyUsernameOrPasswordException)
                    return FubuContinuation.TransferTo(
                        new LoginOutputModel
                            {
                                Username = model.Username,
                                Message = AuthenticationErrorMessage,
                                RedirectUrl = model.RedirectUrl
                            });
                throw;
            }
        }
    }
}