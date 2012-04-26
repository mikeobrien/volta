using System;
using System.IO;
using System.Web;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Arbin;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.IO;
using Volta.Core.Infrastructure.Framework.Security;

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
        private readonly ISecureSession<Token> _secureSession;

        public BatchPostHandler(
            IRepository<Batch> batches, 
            IRepository<ScheduleFile> schedules, 
            ISecureSession<Token> secureSession)
        {
            _batches = batches;
            _schedules = schedules;
            _secureSession = secureSession;
        }

        public void Execute(BatchPostRequest request)
        {
            using (var file = new TempFile(request.file.InputStream))
            using (var arbinData = new ArbinData(file.Path))
            {
                var schedule = new Schedule(_schedules.Get(request.scheduleId).File);
                _batches.Add(new Batch {
                    Name = Path.GetFileNameWithoutExtension(request.file.FileName),
                    CreatedBy = _secureSession.GetCurrentToken().Username,
                    Created = DateTime.Now
                });
            }
        }
    }
}