using System;
using System.Configuration;

namespace Volta.Tests.Acceptance.Common
{
    public class Constants
    {
        public static Uri VoltaUrl = new Uri(ConfigurationManager.AppSettings["VoltaUrl"]);
    }
}