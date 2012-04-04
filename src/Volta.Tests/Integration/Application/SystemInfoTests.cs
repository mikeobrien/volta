using System;
using System.IO;
using System.Reflection;
using NSubstitute;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Application;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Reflection;

namespace Volta.Tests.Integration.Application
{
    [TestFixture]
    public class SystemInfoTests
    {
        private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

        [Test]
        public void should_get_assembly_version()
        {
            var systeminfo = new SystemInfo(Substitute.For<IConnection>(), Substitute.For<ILogger>());
            systeminfo.Version.ShouldEqual(_assembly.GetName().Version.ToString());
        }

        [Test]
        public void should_get_assembly_timestamp()
        {
            var systeminfo = new SystemInfo(Substitute.For<IConnection>(), Substitute.For<ILogger>());
            systeminfo.BuildDate.ToString().ShouldEqual(File.GetLastWriteTime(_assembly.Location).ToString());
        }

        [Test]
        public void should_get_assembly_mode()
        {
            var systeminfo = new SystemInfo(Substitute.For<IConnection>(), Substitute.For<ILogger>());
            systeminfo.DebugMode.ShouldEqual(_assembly.IsInDebugMode() ? SystemInfo.Mode.Debug : SystemInfo.Mode.Release);
        }

        [Test]
        public void should_get_webserver_name()
        {
            var systeminfo = new SystemInfo(Substitute.For<IConnection>(), Substitute.For<ILogger>());
            systeminfo.WebServer.ShouldEqual(Environment.MachineName);
        }

        [Test]
        public void should_get_smtp_server_name()
        {
            var systeminfo = new SystemInfo(Substitute.For<IConnection>(), Substitute.For<ILogger>());
            systeminfo.SmtpServer.ShouldEqual(new System.Net.Mail.SmtpClient().Host);
        }

        [Test]
        public void should_get_database()
        {
            var connection = Substitute.For<IConnection>();
            connection.ConnectionString.Returns("yada");
            var systeminfo = new SystemInfo(connection, Substitute.For<ILogger>());
            systeminfo.Database.ShouldEqual("yada");
        }

        [Test]
        public void should_get_log_config()
        {
            var logger = Substitute.For<ILogger>();
            logger.Path.Returns("path");
            logger.Email.Returns("email");
            logger.SmtpServer.Returns("smtp");
            var systeminfo = new SystemInfo(Substitute.For<IConnection>(), logger);
            systeminfo.LogPath.ShouldEqual("path");
            systeminfo.ErrorEmail.ShouldEqual("email");
            systeminfo.ErrorEmailSmtp.ShouldEqual("smtp");
        }
    }
}