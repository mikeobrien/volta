using System.Net;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Web.Behaviors
{
    public class AjaxAuthorizationBehavior : IActionBehavior
    {
        public const string UnauthorizedMessage = "You need to login to perform this action.";

        private readonly IOutputWriter _writer;
        private readonly IActionBehavior _actionBehavior;
        private readonly ISecureSession<Token> _secureSession;
        private readonly IWebServer _webServer;

        public AjaxAuthorizationBehavior(
            IOutputWriter writer, 
            IActionBehavior actionBehavior, 
            ISecureSession<Token> secureSession, 
            IWebServer webServer)
        {
            _writer = writer;
            _actionBehavior = actionBehavior;
            _secureSession = secureSession;
            _webServer = webServer;
        }

        public void Invoke()
        {
            if (!_secureSession.IsLoggedIn())
            {   
                _webServer.IgnoreErrorStatus = true;
                _writer.WriteResponseCode(HttpStatusCode.Unauthorized, UnauthorizedMessage);
            }
            else _actionBehavior.Invoke();
        }

        public void InvokePartial()
        {
            _actionBehavior.InvokePartial();
        }
    }
}