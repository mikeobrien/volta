using System;

namespace Volta.Core.Domain.Batches
{
    public class ScheduleFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
    }
}
