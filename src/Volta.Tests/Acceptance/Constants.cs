using System;
using System.Configuration;

namespace Volta.Tests.Acceptance
{
    public class Constants
    {
        public static Uri VoltaUrl = new Uri(ConfigurationManager.AppSettings["VoltaUrl"]);
        public static string VoltaConnectionString = ConfigurationManager.ConnectionStrings["VoltaAcceptance"].ConnectionString;
    }
}