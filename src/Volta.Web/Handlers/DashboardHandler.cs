using System;

namespace Volta.Web.Handlers
{
    public class DashboardOutputModel
    {
        public string Time { get; set; }
    }

    public class DashboardHandler
    {
        public DashboardOutputModel Query()
        {
            return new DashboardOutputModel { Time = DateTime.Now.ToString() };
        }
    }
}