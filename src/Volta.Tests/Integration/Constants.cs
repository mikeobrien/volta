using System.Configuration;

namespace Volta.Tests.Integration
{
    public class Constants
    {
        public static string VoltaTestConnectionString = ConfigurationManager.ConnectionStrings["VoltaTest"].ConnectionString;
    }
}