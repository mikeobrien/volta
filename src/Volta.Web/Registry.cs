using FubuMVC.Core.Runtime;
using Volta.Core.Application.Configuration;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Web
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            ForSingletonOf<IConfiguration>().Use<Core.Application.Configuration.Configuration>();
            ForSingletonOf<ILogger>().Use<Log4NetLogger>();
            For<IContentFile>().Use<ContentFile>();

            ForSingletonOf<IIdentityConvention>().Use<IdConvention>();
            For<MongoConnection>().Use(x => new MongoConnection(x.GetInstance<IConfiguration>().ConnectionString));
            For(typeof(IRepository<>)).Use(typeof(MongoRepository<>));

            For<ISecureSession>().Use<SecureSession>();
            For<IAuthenticationService>().Use<AuthenticationService>();

            For<IUserFactory>().Use<UserFactory>();
        }
    }
}