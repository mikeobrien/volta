using System;
using AutoMapper;
using Volta.Core.Domain;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches
{
    public class BatchGetHandler
    {
        private readonly IRepository<Batch> _batches;

        public BatchGetHandler(IRepository<Batch> batches)
        {
            _batches = batches;
        }

        public BatchModel Execute_id(BatchModel request)
        {
            var schedule = _batches.Get(request.id);
            if (schedule == null) throw new NotFoundException("Batch");
            return Mapper.Map<BatchModel>(schedule);
        }
    }
}