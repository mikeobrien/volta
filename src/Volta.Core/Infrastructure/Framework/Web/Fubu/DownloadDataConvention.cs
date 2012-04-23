using FubuCore;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;

namespace Volta.Core.Infrastructure.Framework.Web.Fubu
{
    public class DownloadDataConvention : ActionCallModification
    {
        public DownloadDataConvention()
            : base(call => call.AddToEnd(new OutputNode(typeof (DownloadDataBehavior))), "Adding download data behavior as the output node")
        {
            Filters.Excludes.Add(call => call.HasAnyOutputBehavior());
            Filters.Includes.Add(call => call.OutputType().CanBeCastTo<DownloadDataModel>());
        }
    }
}