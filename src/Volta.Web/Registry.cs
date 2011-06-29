using FubuMVC.Core.Runtime;
using Volta.Core.Application.Configuration;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            For<IConfiguration>().Use<Core.Application.Configuration.Configuration>();

            For<MongoConnection>().Use(x => new MongoConnection(x.GetInstance<IConfiguration>().ConnectionString));
            For<IIdentityConvention>().Use<IdConvention>();
            For(typeof(IRepository<>)).Use(typeof(MongoRepository<>));

            For<ISecureSession>().Use<SecureSession>();
            For<IAuthenticationService>().Use<AuthenticationService>();

            For<IUserFactory>().Use<UserFactory>();
        }
    }
}