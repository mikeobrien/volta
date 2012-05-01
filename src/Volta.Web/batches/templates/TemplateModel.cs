using System;

namespace Volta.Web.Batches.Templates
{
    public class TemplateModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public string createdBy { get; set; }
        public DateTime created { get; set; }
    }
}