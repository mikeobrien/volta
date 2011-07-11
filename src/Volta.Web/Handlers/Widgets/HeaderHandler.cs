using FubuMVC.Core;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Handlers.Widgets
{
    public class HeaderOutputModel
    {
        public string CurrentUser { get; set; }
    }

    public class HeaderHandler
    {
        private readonly ISecureSession<Token> _secureSession;

        public HeaderHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        [FubuPartial]
        public HeaderOutputModel Query(HeaderOutputModel output)
        {
            output.CurrentUser = _secureSession.GetCurrentToken().Username;
            return output;
        }
    }
}