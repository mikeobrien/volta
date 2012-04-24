using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches
{
    public class BatchPutHandler
    {
        private readonly IRepository<Batch> _batches;

        public BatchPutHandler(IRepository<Batch> batches)
        {
            _batches = batches;
        }

        public void Execute_id(BatchModel request)
        {
            _batches.Modify(request.id, x => x
                .Set(y => y.Name, request.name));
        }
    }
}