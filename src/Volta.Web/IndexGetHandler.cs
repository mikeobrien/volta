using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web
{
    public class IndexModel
    {
        public bool isLoggedIn { get; set; }
    }

    public class IndexGetHandler
    {
        private readonly ISecureSession<Token> _secureSession;

        public IndexGetHandler(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public IndexModel Execute()
        {
            return new IndexModel
                {
                    isLoggedIn = _secureSession.IsLoggedIn()
                };
        }
    }
}