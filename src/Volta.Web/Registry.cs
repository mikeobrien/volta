using Volta.Core.Application;
using Volta.Core.Application.Configuration;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Core.Infrastructure.Framework.Web.FubuMvc;
using Volta.Core.Infrastructure.Framework.Web.Navigation;

namespace Volta.Web
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            ForSingletonOf<IApplication>().Use<Application>();
            ForSingletonOf<IConfiguration>().Use<Core.Application.Configuration.Configuration>();
            ForSingletonOf<ILogger>().Use<Log4NetLogger>();

            For<MongoConnection>().Use(x => new MongoConnection(x.GetInstance<IConfiguration>().ConnectionString));
            For(typeof(IRepository<>)).Use(typeof(MongoRepository<>));

            For<ITokenStore<Token>>().Use<FubuTokenStore<Token>>();
            For<ISecureSession<Token>>().Use<SecureSession<Token>>();
            For<IAuthenticationService<Token>>().Use<AuthenticationService>();

            For<IUserCreationService>().Use<UserCreationService>();
            For<IUserModificationService>().Use<UserModificationService>();

            For<TabCollection>().Use<Navigation>();
        }
    }
}