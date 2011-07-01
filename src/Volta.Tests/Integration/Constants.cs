using System.Configuration;

namespace Volta.Tests.Integration
{
    public class Constants
    {
        public static string VoltaConnectionString = ConfigurationManager.ConnectionStrings["VoltaIntegration"].ConnectionString;
    }
}