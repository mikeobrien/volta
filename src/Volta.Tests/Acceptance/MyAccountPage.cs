using System;
using Volta.Tests.Acceptance.Common;
using Volta.Web.Controllers.Login;
using WatiN.Core;

namespace Volta.Tests.Acceptance
{
    public class MyAccountPage : VoltaWebPage
    {
        public MyAccountPage()
        {
            BaseUrl = new Uri(Constants.VoltaUrl, "/myaccount");
        }

    }
}