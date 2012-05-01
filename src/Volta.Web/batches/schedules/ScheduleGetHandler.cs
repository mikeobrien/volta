using System;
using AutoMapper;
using Volta.Core.Domain;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Schedules
{
    public class ScheduleGetHandler
    {
        private readonly IRepository<ScheduleFile> _schedules;

        public ScheduleGetHandler(IRepository<ScheduleFile> schedules)
        {
            _schedules = schedules;
        }

        public ScheduleModel Execute_id(ScheduleModel request)
        {
            var schedule = _schedules.Get(request.id);
            if (schedule == null) throw new NotFoundException("Schedule");
            return Mapper.Map<ScheduleModel>(schedule);
        }
    }
}