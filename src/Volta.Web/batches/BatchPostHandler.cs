using System;
using System.Web;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Arbin;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.IO;

namespace Volta.Web.Batches
{
    public class BatchPostRequest
    {
        public HttpPostedFileBase file { get; set; }
    }

    public class BatchPostResponse
    {
        public Guid id { get; set; }
    }

    public class BatchPostHandler
    {
        private readonly IRepository<Batch> _batches;
        private readonly BatchFactory _batchFactory;

        public BatchPostHandler(
            IRepository<Batch> batches, 
            BatchFactory batchFactory)
        {
            _batches = batches;
            _batchFactory = batchFactory;
        }

        public BatchPostResponse Execute(BatchPostRequest request)
        {
            using (var arbinFile = new TempFile(request.file.InputStream))
            using (var arbinData = new ArbinData(arbinFile.Path))
            {
                var batch = _batchFactory.Create(arbinData);
                _batches.Add(batch);
                return new BatchPostResponse { id = batch.Id };
            }
        }
    }
}