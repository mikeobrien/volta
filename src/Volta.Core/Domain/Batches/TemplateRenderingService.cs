using System;
using System.IO;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using RazorEngine.Templating;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Latex;
using Volta.Core.Infrastructure.Framework.Razor;

namespace Volta.Core.Domain.Batches
{
    public class TemplateRenderingService
    {
        public class RenderException : Exception
        {
            public RenderException(string details) : base("A rendering error has occured.")
            {
                Details = details;
            }

            public string Details { get; set; }
        }

        private readonly IRepository<Batch> _batches;
        private readonly IRepository<Template> _templates;
        private readonly ILatexEngine _latexEngine;
        private readonly IRazorEngine _razorEngine;

        public TemplateRenderingService(IRepository<Batch> batches, IRepository<Template> templates, ILatexEngine latexEngine, IRazorEngine razorEngine)
        {
            _batches = batches;
            _templates = templates;
            _latexEngine = latexEngine;
            _razorEngine = razorEngine;
        }

        public string RenderLatex(Guid templateId, Guid batchId)
        {
            var template = _templates.Get(templateId);
            var batch = _batches.Get(batchId);
            try
            {
                return _razorEngine.Transform(template.Source, new {Batch = batch});
            }
            catch (TemplateCompilationException exception)
            {
                throw new RenderException(string.Format("{0}\r\n\r\n{1}", exception.Message, exception
                    .Errors.Select(x => x.ErrorText)
                    .Aggregate((a, i) => a + "\r\n\r\n" + i)));
            }
            catch(RuntimeBinderException exception)
            {
                throw new RenderException(exception.Message);
            }
        }

        public string RenderPdf(Guid templateId, Guid batchId)
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), Guid.NewGuid() + ".tex");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, RenderLatex(templateId, batchId));
            string outputPath;
            try
            {
                outputPath = _latexEngine.GeneratePdf(path, new LatexOptions { OutputDirectory = Path.GetDirectoryName(path)});
            }
            catch (LatexException exception)
            {
                Directory.Delete(Path.GetDirectoryName(path), true);
                throw new RenderException(exception.Output);
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
            return outputPath;
        }
    }
}