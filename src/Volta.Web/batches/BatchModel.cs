using System;

namespace Volta.Web.Batches
{
    public class BatchModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string gloveBox { get; set; }
        public double operatingTemperature { get; set; }
        public double cyclingCurrent { get; set; }
        public Guid scheduleId { get; set; }
        public string createdBy { get; set; }
        public DateTime created { get; set; }
    }
}