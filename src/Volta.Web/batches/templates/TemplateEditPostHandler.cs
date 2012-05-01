using System;
using System.Text;
using System.Web;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Templates
{
    public class TemplateEditPostRequest
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public HttpPostedFileBase file { get; set; }
    }

    public class TemplateEditPostHandler
    {
        private readonly IRepository<Template> _templates;

        public TemplateEditPostHandler(IRepository<Template> templates)
        {
            _templates = templates;
        }

        public void Execute_id(TemplateEditPostRequest request)
        {
            string source;
            if (request.file != null)
            {
                var file = new byte[request.file.ContentLength];
                request.file.InputStream.Read(file, 0, file.Length);
                source = Encoding.ASCII.GetString(file);
            }
            else source = request.source;

            _templates.Modify(request.id, x => x
                .Set(y => y.Name, request.name)
                .Set(y => y.Source, source));
        }
    }
}