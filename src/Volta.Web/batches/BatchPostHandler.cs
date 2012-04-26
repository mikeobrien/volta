using System;
using System.IO;
using System.Web;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Arbin;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.IO;

namespace Volta.Web.Batches
{
    public class BatchPostRequest
    {
        public Guid scheduleId { get; set; }
        public HttpPostedFileBase file { get; set; }
    }

    public class BatchPostHandler
    {
        private readonly IRepository<Batch> _batches;
        private readonly IRepository<ScheduleFile> _schedules;
        private readonly BatchFactory _batchFactory;

        public BatchPostHandler(
            IRepository<Batch> batches, 
            IRepository<ScheduleFile> schedules,
            BatchFactory batchFactory)
        {
            _batches = batches;
            _schedules = schedules;
            _batchFactory = batchFactory;
        }

        public void Execute(BatchPostRequest request)
        {
            using (var arbinFile = new TempFile(request.file.InputStream))
            using (var arbinData = new ArbinData(arbinFile.Path))
            {
                _batches.Add(_batchFactory.Create(
                    Path.GetFileNameWithoutExtension(request.file.FileName), 
                    new Schedule(_schedules.Get(request.scheduleId).File),
                    arbinData));
            }
        }
    }
}