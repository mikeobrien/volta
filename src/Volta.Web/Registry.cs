using Volta.Core.Application;
using Volta.Core.Application.Configuration;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Web
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            ForSingletonOf<IApplication>().Use<Application>();
            ForSingletonOf<IConfiguration>().Use<Configuration>();
            ForSingletonOf<ILogger>().Use<Log4NetLogger>();
            ForSingletonOf<IWebServer>().Use<WebServer>();

            For<MongoConnection>().Use(x => new MongoConnection(x.GetInstance<IConfiguration>().ConnectionString));
            For(typeof(IRepository<>)).Use(typeof(MongoRepository<>));

            For<ISession>().Use<Session>();
            For<ITokenStore<Token>>().Use<TokenStore<Token>>();
            For<ISecureSession<Token>>().Use<SecureSession<Token>>();
            For<IAuthenticationService<Token>>().Use<AuthenticationService>();

            For<IUserCreationService>().Use<UserCreationService>();
            For<IUserModificationService>().Use<UserModificationService>();
        }
    }
}