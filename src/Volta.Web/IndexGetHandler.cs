using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Application;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web
{
    public class IndexModel
    {
        public string Username { get; set; }
        public bool IsAdministrator { get; set; }
        public ISystemInfo SystemInfo { get; set; }
    }

    public class IndexGetHandler
    {
        private readonly ISecureSession<Token> _secureSession;
        private readonly ISystemInfo _systemInfo;

        public IndexGetHandler(ISecureSession<Token> secureSession, ISystemInfo systemInfo)
        {
            _secureSession = secureSession;
            _systemInfo = systemInfo;
        }

        public IndexModel Execute()
        {
            var token = _secureSession.GetCurrentToken();
            return new IndexModel
                {
                    Username = token.Username,
                    IsAdministrator = token.IsAdministrator,
                    SystemInfo = _systemInfo
                };
        }
    }
}