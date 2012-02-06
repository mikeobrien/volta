using System;
using FubuMVC.Core;
using Volta.Core.Application;

namespace Volta.Web.Handlers.Widgets
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
        public FooterOutputModel Query(FooterOutputModel output)
        {
            output.Version = _application.Version;
            output.ReleaseDate = _application.ReleaseDate.ToString("MM/dd/yyyy hh:mm:ss");
            output.CopyrightYear = _application.CopyrightYear;
            return output;
        }
    }
}