using System;
using FubuCore;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using FubuMVC.Core.Continuations;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Application;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Handlers
{
    public class LoginOutputModel : ComparableModelBase
    {
        public MessageModel Message { get; set; }
        public string Username { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class LoginInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RedirectUrl { get; set; }
        public string Action { get; set; }
    }

    public class LoginHandler
    {
        public const string NotAuthenticatedMessage = "You need to login to access this resource.";
        public const string InvalidUsernameOrPasswordMessage = "Invalid username or password.";

        private readonly ISecureSession<Token> _secureSession;

        public LoginHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public LoginOutputModel Query(LoginOutputModel output)
        {
            if (!output.RedirectUrl.IsEmpty() && (output.Message == null || !output.Message.HasMessageText))
                output.Message =  MessageModel.Information(NotAuthenticatedMessage);
            return output;
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
                                Message = MessageModel.Information(InvalidUsernameOrPasswordMessage),
                                RedirectUrl = input.RedirectUrl
                            });
                throw;
            }
        }
    }
}