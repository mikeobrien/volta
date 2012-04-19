using System;
using AutoMapper;
using Volta.Core.Domain;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Schedules
{
    public class ScheduleGetRequest
    {
        public Guid Id { get; set; }
    }

    public class ScheduleGetHandler
    {
        private readonly IRepository<ScheduleFile> _schedules;

        public ScheduleGetHandler(IRepository<ScheduleFile> schedules)
        {
            _schedules = schedules;
        }

        public ScheduleModel Execute_Id(ScheduleGetRequest request)
        {
            var schedule = _schedules.Get(request.Id);
            if (schedule == null) throw new NotFoundException("Schedule");
            return Mapper.Map<ScheduleModel>(schedule);
        }
    }
}