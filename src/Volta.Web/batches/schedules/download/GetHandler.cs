using System;
using System.Text;
using Volta.Core.Domain;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Web.Fubu;

namespace Volta.Web.Batches.Schedules.Download
{
    public class GetRequest
    {
        public Guid Id { get; set; }
    }

    public class GetHandler
    {
        private readonly IRepository<ScheduleFile> _schedules;

        public GetHandler(IRepository<ScheduleFile> schedules)
        {
            _schedules = schedules;
        }

        public DownloadDataModel Execute_Id(ScheduleGetRequest request)
        {
            var schedule = _schedules.Get(request.Id);
            if (schedule == null) throw new NotFoundException("Schedule");
            return new DownloadDataModel {
                Data = Encoding.GetEncoding(1252).GetBytes(schedule.File), 
                Filename = schedule.Name + ".sdu", 
                MimeType = "text/plain; charset=windows-1252"
            };
        }
    }
}