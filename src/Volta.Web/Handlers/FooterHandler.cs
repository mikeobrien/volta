using FubuMVC.Core;
using Volta.Core.Application;

namespace Volta.Web.Handlers
{
    public class FooterOutputModel
    {
        public string Version { get; set; }
        public string ReleaseDate { get; set; }
        public string CopyrightYear { get; set; }
    }

    public class FooterHandler
    {
        private readonly IApplication _application;

        public FooterHandler(IApplication application)
        {
            _application = application;
        }

        [FubuPartial]
        public FooterOutputModel Query(FooterOutputModel model)
        {
            return new FooterOutputModel
                       {
                           Version = _application.Version,
                           ReleaseDate = _application.ReleaseDate.ToString("MM/dd/yyyy hh:mm:ss"),
                           CopyrightYear = _application.CopyrightYear
                       };
        }
    }
}