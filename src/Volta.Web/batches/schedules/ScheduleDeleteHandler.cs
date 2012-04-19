using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Schedules
{
    public class ScheduleDeleteHandler
    {
        private readonly IRepository<ScheduleFile> _schedules;

        public ScheduleDeleteHandler(IRepository<ScheduleFile> schedules)
        {
            _schedules = schedules;
        }

        public void Execute_id(ScheduleModel request)
        {
            _schedules.Delete(request.id);
        }
    }
}