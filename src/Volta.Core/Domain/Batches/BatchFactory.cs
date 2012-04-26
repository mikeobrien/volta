using System;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Arbin;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Batches
{
    public class BatchFactory
    {
        private readonly ISecureSession<Token> _secureSession;

        public BatchFactory(ISecureSession<Token> secureSession)
        {
            _secureSession = secureSession;
        }

        public Batch Create(string name, Schedule schedule, IArbinData arbinData)
        {
            var batch = new Batch();
            batch.BatchId = name;
            batch.CreatedBy = _secureSession.GetCurrentToken().Username;
            batch.Created = DateTime.Now;
            return batch;
        }
    }
}