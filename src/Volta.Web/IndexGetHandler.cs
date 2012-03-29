using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web
{
    public class IndexModel
    {
        public string Username { get; set; }
        public bool IsAdministrator { get; set; }
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
            var token = _secureSession.GetCurrentToken();
            return new IndexModel
                {
                    Username = token.Username,
                    IsAdministrator = token.IsAdministrator,
                };
        }
    }
}