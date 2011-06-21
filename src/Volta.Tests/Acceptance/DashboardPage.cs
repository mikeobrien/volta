using System;
using Volta.Tests.Acceptance.Common;
using Volta.Web.Controllers.Login;
using WatiN.Core;

namespace Volta.Tests.Acceptance
{
    public class DashboardPage : VoltaWebPage
    {
        public DashboardPage()
        {
            BaseUrl = new Uri(Constants.VoltaUrl, "/");
        }

    }
}