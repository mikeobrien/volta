using System;
using System.Collections.Generic;
using System.Linq;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Application;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web
{
    public class IndexModel
    {
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public bool IsAdministrator { get; set; }
        public ISystemInfo SystemInfo { get; set; }
        public IEnumerable<ScheduleFile> Schedules { get; set; }
        public IEnumerable<Batch> Batches { get; set; }
    }

    public class IndexGetHandler
    {
        private readonly ISecureSession<Token> _secureSession;
        private readonly ISystemInfo _systemInfo;
        private readonly IRepository<ScheduleFile> _schedules;
        private readonly IRepository<Batch> _batches;

        public IndexGetHandler(
            ISecureSession<Token> secureSession, 
            ISystemInfo systemInfo,
            IRepository<ScheduleFile> schedules,
            IRepository<Batch> batches)
        {
            _secureSession = secureSession;
            _systemInfo = systemInfo;
            _schedules = schedules;
            _batches = batches;
        }

        public IndexModel Execute()
        {
            var token = _secureSession.GetCurrentToken();
            return new IndexModel
                {
                    UserId = token.UserId,
                    Username = token.Username,
                    IsAdministrator = token.IsAdministrator,
                    SystemInfo = _systemInfo,
                    Schedules = _schedules.OrderBy(x => x.Created).Take(5),
                    Batches = _batches.OrderBy(x => x.Created).Take(5)
                };
        }
    }
}