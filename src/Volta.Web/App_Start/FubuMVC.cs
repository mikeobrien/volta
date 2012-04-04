using Bottles;
using Volta.Core.Infrastructure.Application.Data;
using Volta.Web.App_Start;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AppStartFubuMvc), "Start")]

namespace Volta.Web.App_Start
{
    public static class AppStartFubuMvc
    {
        public static void Start()
        {
            FubuApplication.For<FubuConventions>().StructureMap(new Container(new Registry())).Bootstrap();
            MongoConventions.Register();
            MappingConventions.Register();
            PackageRegistry.AssertNoFailures();
        }
    }
}