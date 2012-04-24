using System;
using AutoMapper;
using Volta.Core.Domain;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches
{
    public class BatchGetRequest
    {
        public Guid Id { get; set; }
    }

    public class BatchGetHandler
    {
        private readonly IRepository<Batch> _batches;

        public BatchGetHandler(IRepository<Batch> batches)
        {
            _batches = batches;
        }

        public BatchModel Execute_Id(BatchGetRequest request)
        {
            var schedule = _batches.Get(request.Id);
            if (schedule == null) throw new NotFoundException("Batch");
            return Mapper.Map<BatchModel>(schedule);
        }
    }
}