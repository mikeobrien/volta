using System;
using System.Text;
using Volta.Core.Domain;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Web.Fubu;

namespace Volta.Web.Batches.Templates
{
    public class DownloadGetHandler
    {
        private readonly IRepository<Template> _templates;

        public DownloadGetHandler(IRepository<Template> templates)
        {
            _templates = templates;
        }

        public DownloadDataModel ExecuteDownload_id(TemplateModel request)
        {
            var template = _templates.Get(request.id);
            if (template == null) throw new NotFoundException("Template");
            return new DownloadDataModel {
                Data = Encoding.ASCII.GetBytes(template.Source), 
                Filename = template.Name + ".tex",
                MimeType = "text/plain; charset=us-ascii"
            };
        }
    }
}