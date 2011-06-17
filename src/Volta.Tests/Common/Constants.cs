namespace Volta.Tests.Common
{
    public class Constants
    {
        /*  
            mongo
            > use admin
            > db.auth("admin","admin")
            > use VoltaTest
            > db.addUser("volta","volta")
        */

        public const string VoltaTestConnectionString = "mongodb://volta:volta@localhost/voltatest";
    }
}