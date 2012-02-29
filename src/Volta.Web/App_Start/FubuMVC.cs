using System.Web.Routing;
using Bottles;
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
            FubuApplication.For<Conventions>()
                .StructureMap(new Container(new Registry()))
                .Bootstrap();
            PackageRegistry.AssertNoFailures();
        }
    }
}