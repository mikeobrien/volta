using System.Configuration;

namespace Volta.Tests.Integration
{
    public class Constants
    {
        public static string VoltaAcceptanceConnectionString = ConfigurationManager.ConnectionStrings["VoltaAcceptance"].ConnectionString;
        public static string VoltaIntegrationConnectionString = ConfigurationManager.ConnectionStrings["VoltaIntegration"].ConnectionString;
    }
}