using System;
using System.IO;
using System.Web;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;
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
        private readonly ISecureSession<Token> _secureSession;

        public BatchPostHandler(
            IRepository<Batch> batches, 
            ISecureSession<Token> secureSession)
        {
            _batches = batches;
            _secureSession = secureSession;
        }

        public void Execute(BatchPostRequest request)
        {
            _batches.Add(new Batch {
                Name = Path.GetFileNameWithoutExtension(request.file.FileName),
                CreatedBy = _secureSession.GetCurrentToken().Username,
                Created = DateTime.Now
            });
        }
    }
}