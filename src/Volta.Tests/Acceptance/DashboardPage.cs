using System;
using Volta.Tests.Acceptance.Common;
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