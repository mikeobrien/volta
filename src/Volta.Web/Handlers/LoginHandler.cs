using System;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using Volta.Core.Infrastructure;
using FubuMVC.Core.Continuations;

namespace Volta.Web.Handlers
{
    public class LoginInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Action { get; set; }
    }

    public class LoginOutputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }

        public override bool Equals(object obj) { return this.ObjectEquals(obj); }
        public override int GetHashCode() { return this.ObjectHashCode(Username, Password, Message); }
    }

    public class LoginHandler
    {
        private readonly ISecureSession _secureSession;

        public LoginHandler(ISecureSession secureSession)
        {
            _secureSession = secureSession;
        }

        public LoginOutputModel Query(LoginOutputModel loginOutputModel)
        {
            return loginOutputModel;
        }

        public FubuContinuation Command(LoginInputModel loginInputModel)
        {
            try
            {
                _secureSession.Login(loginInputModel.Username, loginInputModel.Password);
                return FubuContinuation.RedirectTo<DashboardHandler>(x => x.Query());
            }
            catch (Exception e)
            {
                if (e is AccessDeniedException || 
                    e is EmptyUsernameOrPasswordException)
                    return FubuContinuation.TransferTo(
                        new LoginOutputModel
                            {
                                Username = loginInputModel.Username,
                                Message = "Invalid username or password."
                            });
                throw;
            }
        }
    }
}