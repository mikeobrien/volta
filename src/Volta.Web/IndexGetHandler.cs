using System;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Application;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web
{
    public class IndexModel
    {
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public bool IsAdministrator { get; set; }
        public ISystemInfo SystemInfo { get; set; }
        public DashboardModel Dashboard { get; set; }
    }

    public class IndexGetHandler
    {
        private readonly ISecureSession<Token> _secureSession;
        private readonly DashboardGetHandler _dashboard;
        private readonly ISystemInfo _systemInfo;

        public IndexGetHandler(
            ISecureSession<Token> secureSession,
            DashboardGetHandler dashboard,
            ISystemInfo systemInfo)
        {
            _secureSession = secureSession;
            _dashboard = dashboard;
            _systemInfo = systemInfo;
        }

        public IndexModel Execute()
        {
            var token = _secureSession.GetCurrentToken();
            return new IndexModel
                {
                    UserId = token.UserId,
                    Username = token.Username,
                    IsAdministrator = token.IsAdministrator,
                    Dashboard = _dashboard.ExecuteDashboard(),
                    SystemInfo = _systemInfo
                };
        }
    }
}