using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Schedules
{
    public class SchedulePutHandler
    {
        private readonly IRepository<ScheduleFile> _schedules;

        public SchedulePutHandler(IRepository<ScheduleFile> schedules)
        {
            _schedules = schedules;
        }

        public void Execute_id(ScheduleModel request)
        {
            _schedules.Modify(request.id, x => x
                .Set(y => y.Name, request.name)
                .Set(y => y.File, request.file));
        }
    }
}