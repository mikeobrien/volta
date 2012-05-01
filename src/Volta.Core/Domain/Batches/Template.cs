using System;

namespace Volta.Core.Domain.Batches
{
    public class Template
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
    }
}
