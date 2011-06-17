using System;

namespace Volta.Web.Controllers.Dashboard
{
    public class DashboardController
    {
        public DashboardViewModel Index()
        {
            return new DashboardViewModel {Time = DateTime.Now.ToString()};
        }
    }

    public class DashboardViewModel
    {
        public string Time { get; set; }
    }
}