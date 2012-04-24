using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches
{
    public class BatchDeleteHandler
    {
        private readonly IRepository<Batch> _batches;

        public BatchDeleteHandler(IRepository<Batch> batches)
        {
            _batches = batches;
        }

        public void Execute_id(BatchModel request)
        {
            _batches.Delete(request.id);
        }
    }
}