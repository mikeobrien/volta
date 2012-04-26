using System.Collections.Generic;
using System.Linq;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches
{
    public class BatchesGetRequest
    {
        public int Index { get; set; }
    }

    public class BatchesGetHandler
    {
        private readonly IRepository<Batch> _batches;
        public const int PageSize = 20;

        public BatchesGetHandler(IRepository<Batch> batches)
        {
            _batches = batches;
        }

        public List<BatchModel> Execute(BatchesGetRequest request)
        {
            return _batches.OrderBy(x => x.Name).Page(request.Index, PageSize).
                Select(x => new BatchModel {
                                    id = x.Id, 
                                    name = x.Name, 
                                    createdBy = x.CreatedBy, 
                                    created = x.Created
                                }).ToList();
        }
    }
}