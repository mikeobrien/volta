using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;

namespace Volta.Core.Infrastructure.Framework.Web.Fubu
{
    public class DownloadDataBehavior : BasicBehavior
    {
        private readonly IFubuRequest _request;
        private readonly IOutputWriter _writer;

        public DownloadDataBehavior(IOutputWriter writer, IFubuRequest request)
            : base(PartialBehavior.Ignored)
        {
            _writer = writer;
            _request = request;
        }

        protected override DoNext performInvoke()
        {
            var output = _request.Get<DownloadDataModel>();
            switch (output.Type)
            {
                case DownloadDataModel.DownloadType.Data:
                    _writer.Write(output.MimeType, x => x.Write(output.Data, 0, output.Data.Length));   
                    _writer.AppendHeader("Content-Disposition", "attachment; filename=" + output.Filename + ";");
                    break;
                case DownloadDataModel.DownloadType.File:
                    _writer.WriteFile(output.MimeType, output.Path, output.Filename); 
                    break;
            }

            return DoNext.Continue;
        }
    }
}