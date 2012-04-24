using System;
using System.Text;
using System.Web;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Batches.Schedules
{
    public class SchedulePostRequest
    {
        public string name { get; set; }
        public HttpPostedFileBase file { get; set; }
    }

    public class SchedulePostHandler
    {
        private readonly IRepository<ScheduleFile> _schedules;
        private readonly ISecureSession<Token> _secureSession;

        public SchedulePostHandler(
            IRepository<ScheduleFile> schedules, 
            ISecureSession<Token> secureSession)
        {
            _schedules = schedules;
            _secureSession = secureSession;
        }

        public void Execute(SchedulePostRequest request)
        {
            var file = new byte[request.file.ContentLength];
            request.file.InputStream.Read(file, 0, file.Length);
            _schedules.Add(new ScheduleFile {
                Name = request.name,
                File = Encoding.GetEncoding(1252).GetString(file),
                CreatedBy = _secureSession.GetCurrentToken().Username,
                Created = DateTime.Now
            });
        }
    }
}