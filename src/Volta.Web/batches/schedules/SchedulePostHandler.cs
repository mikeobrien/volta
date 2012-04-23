using System.Text;
using System.Web;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

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

        public SchedulePostHandler(IRepository<ScheduleFile> schedules)
        {
            _schedules = schedules;
        }

        public void Execute(SchedulePostRequest request)
        {
            var file = new byte[request.file.ContentLength];
            request.file.InputStream.Read(file, 0, file.Length);
            _schedules.Add(new ScheduleFile {
                Name = request.name,
                File = Encoding.GetEncoding(1252).GetString(file) 
            });
        }
    }
}