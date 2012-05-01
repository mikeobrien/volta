using System;
using System.Text;
using System.Web;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Web.Batches.Templates
{
    public class TemplatePostRequest
    {
        public string name { get; set; }
        public HttpPostedFileBase file { get; set; }
    }

    public class TemplatePostHandler
    {
        private readonly IRepository<Template> _templates;
        private readonly ISecureSession<Token> _secureSession;

        public TemplatePostHandler(
            IRepository<Template> templates, 
            ISecureSession<Token> secureSession)
        {
            _templates = templates;
            _secureSession = secureSession;
        }

        public void Execute(TemplatePostRequest request)
        {
            var file = new byte[request.file.ContentLength];
            request.file.InputStream.Read(file, 0, file.Length);
            _templates.Add(new Template
            {
                Name = request.name,
                Source = Encoding.ASCII.GetString(file),
                CreatedBy = _secureSession.GetCurrentToken().Username,
                Created = DateTime.Now
            });
        }
    }
}