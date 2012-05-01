using Volta.Core.Application.Security;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Application;
using Volta.Core.Infrastructure.Application.Configuration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Latex;
using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Razor;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Web
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            ForSingletonOf<ISystemInfo>().Use<SystemInfo>();
            ForSingletonOf<IConfiguration>().Use<Configuration>();
            ForSingletonOf<ILogger>().Use<Log4NetLogger>();

            For<IConnection>().Use<MongoConnection>().Ctor<string>().Is(x => x.GetInstance<IConfiguration>().ConnectionString);
            For(typeof(IRepository<>)).Use(typeof(MongoRepository<>));

            ForSingletonOf<IWebServer>().Use<WebServer>();
            For<ISession>().Use<Session>();
            For<ITokenStore<Token>>().Use<TokenStore<Token>>();
            For<ISecureSession<Token>>().Use<SecureSession<Token>>();
            For<IAuthenticationService<Token>>().Use<AuthenticationService>();

            For<IUserFactory>().Use<UserCreateService>();
            For<IUserUpdateService>().Use<UserUpdateService>();
            For<IUserDeleteService>().Use<UserDeleteService>();

            For<ILatexEngine>().Use<LatexEngine>();
            For<IRazorEngine>().Use<RazorEngine>();
        }
    }
}