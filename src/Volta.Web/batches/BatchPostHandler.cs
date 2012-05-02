using System;
using System.Web;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Arbin;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.IO.FileStore;

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
        private readonly IFileStore _fileStore;

        public BatchPostHandler(
            IRepository<Batch> batches, 
            BatchFactory batchFactory, 
            IFileStore fileStore)
        {
            _batches = batches;
            _batchFactory = batchFactory;
            _fileStore = fileStore;
        }

        public BatchPostResponse Execute(BatchPostRequest request)
        {
            var dataId = _fileStore.Save(request.file.InputStream, Lifespan.Transient);
            Batch batch;
            using (var arbinData = new ArbinData(_fileStore.GetPath(dataId)))
            {
                batch = _batchFactory.Create(arbinData);
                _batches.Add(batch);
            }
            _fileStore.Delete(dataId);
            return new BatchPostResponse { id = batch.Id };
        }
    }
}