using System;
using System.Linq;
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

        public Batch Create(ScheduleFile scheduleFile, IArbinData arbinData)
        {
            var global = arbinData.GetGlobal().ToList();
            var batch = new Batch();
            batch.Name = global.First().TestName;
            batch.CreatedBy = _secureSession.GetCurrentToken().Username;
            batch.Created = DateTime.Now;
            batch.Cells.AddRange(global.Select(x => new Cell()));
            var primaryOperator = global.First().Creator;
            batch.ComponentPhase.Operator = primaryOperator;
            batch.AssemblyPhase.Operator = primaryOperator;
            batch.OperationPhase.Operator = primaryOperator;
            batch.ScheduleId = scheduleFile.Id;
            batch.ScheduleName = scheduleFile.Name;
            return batch;
        }
    }
}