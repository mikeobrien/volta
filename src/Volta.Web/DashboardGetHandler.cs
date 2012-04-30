using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Application;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web.Batches;
using Volta.Web.Batches.Schedules;

namespace Volta.Web
{
    public class DashboardModel
    {
        public string version { get; set; }
        public string buildDate { get; set; }
        public string username { get; set; }
        public string userType { get; set; }
        public IEnumerable<ScheduleModel> schedules { get; set; }
        public IEnumerable<BatchModel> batches { get; set; }
    }

    public class DashboardGetHandler
    {
        private readonly ISecureSession<Token> _secureSession;
        private readonly ISystemInfo _systemInfo;
        private readonly IRepository<ScheduleFile> _schedules;
        private readonly IRepository<Batch> _batches;

        public DashboardGetHandler(
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

        public DashboardModel ExecuteDashboard()
        {
            var token = _secureSession.GetCurrentToken();
            return new DashboardModel
                {
                    version = _systemInfo.Version,
                    buildDate = _systemInfo.BuildDate.ToString("MM/dd/yyyy"),
                    username = token.Username,
                    userType = token.IsAdministrator ? "Administrator" : "User",
                    schedules = Mapper.Map<IEnumerable<ScheduleModel>>(_schedules.OrderBy(x => x.Created).Take(5)),
                    batches = Mapper.Map<IEnumerable<BatchModel>>(_batches.OrderBy(x => x.Created).Take(5))
                };
        }
    }
}