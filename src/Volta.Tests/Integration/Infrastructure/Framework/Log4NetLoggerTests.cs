using System.IO;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Logging;

namespace Volta.Tests.Integration.Infrastructure.Framework
{
    [TestFixture]
    public class Log4NetLoggerTests
    {
        [Test]
        public void should_return_email_config()
        {
            var logger = new Log4NetLogger();
            logger.Email.ShouldEqual("errors@test.com");
            logger.SmtpServer.ShouldEqual("smtp.test.com");
        }

        [Test]
        public void should_return_file_config()
        {
            var logger = new Log4NetLogger();
            logger.Path.ShouldEqual(Path.GetFullPath("Volta.Web.log"));
        }
    }
}