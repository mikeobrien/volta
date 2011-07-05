using FubuMVC.Core;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Handlers
{
    public class HeaderLinksOutputModel
    {
        public string CurrentUser { get; set; }
    }

    public class HeaderLinksHandler
    {
        private readonly ISecureSession<Token> _secureSession;

        public HeaderLinksHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        [FubuPartial]
        public HeaderLinksOutputModel Query(HeaderLinksOutputModel model)
        {
            return new HeaderLinksOutputModel
                       {
                           CurrentUser = _secureSession.GetCurrentToken().Username
                       };
        }
    }
}