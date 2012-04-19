using AutoMapper;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Schedules
{
    public class SchedulePostHandler
    {
        private readonly IRepository<ScheduleFile> _schedules;

        public SchedulePostHandler(IRepository<ScheduleFile> schedules)
        {
            _schedules = schedules;
        }

        public void Execute(ScheduleModel request)
        {
            _schedules.Add(Mapper.Map<ScheduleFile>(request));
        }
    }
}