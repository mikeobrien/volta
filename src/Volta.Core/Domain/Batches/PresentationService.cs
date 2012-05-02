using System;
using System.IO;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using RazorEngine.Templating;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.IO.FileStore;
using Volta.Core.Infrastructure.Framework.Latex;
using Volta.Core.Infrastructure.Framework.Razor;

namespace Volta.Core.Domain.Batches
{
    public class PresentationService
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
        private readonly IFileStore _fileStore;

        public PresentationService(
            IRepository<Batch> batches, 
            IRepository<Template> templates, 
            ILatexEngine latexEngine, 
            IRazorEngine razorEngine,
            IFileStore fileStore)
        {
            _batches = batches;
            _templates = templates;
            _latexEngine = latexEngine;
            _razorEngine = razorEngine;
            _fileStore = fileStore;
        }


        public Guid RenderLatex(Guid templateId, Guid batchId)
        {
            return _fileStore.Save(RenderLatexTemplate(templateId, batchId), Lifespan.Transient);
        }

        private string RenderLatexTemplate(Guid templateId, Guid batchId)
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

        public Guid RenderPdf(Guid templateId, Guid batchId)
        {
            var file = _fileStore.Create(Lifespan.Transient);
            var latexFile = file.Path + ".tex";
            File.WriteAllText(latexFile, RenderLatexTemplate(templateId, batchId));
            try
            {
                var outputPath = _latexEngine.GeneratePdf(latexFile, new LatexOptions {
                        OutputDirectory = Path.GetDirectoryName(file.Path), 
                        Interaction = LatexOptions.InteractionMode.NonStopMode
                    });
                File.Move(outputPath, file.Path);
            }
            catch (LatexException exception)
            {
                throw new RenderException(exception.Output);
            }
            finally
            {
                if (File.Exists(latexFile)) File.Delete(latexFile);
            }
            return file.Id;
        }
    }
}