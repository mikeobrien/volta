using System;

namespace Volta.Web.Batches
{
    public class BatchModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string createdBy { get; set; }
        public DateTime created { get; set; }
    }
}