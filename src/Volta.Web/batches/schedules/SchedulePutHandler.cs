using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Schedules
{
    public class UserPutHandler
    {
        private readonly IRepository<ScheduleFile> _schedules;

        public UserPutHandler(IRepository<ScheduleFile> schedules)
        {
            _schedules = schedules;
        }

        public void Execute_id(ScheduleModel request)
        {
            _schedules.Modify(request.id, new { request.name, request.file });
        }
    }
}