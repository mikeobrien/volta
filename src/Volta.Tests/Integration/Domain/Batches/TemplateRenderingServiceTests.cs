using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Should;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Latex;
using Volta.Core.Infrastructure.Framework.Razor;
using Volta.Tests.Unit;

namespace Volta.Tests.Integration.Domain.Batches
{
    [TestFixture]
    public class TemplateRenderingServiceTests
    {
        private const string Template = @"\documentclass[12pt]{article}
\begin{document}
  Hello @Model.Batch.Name.
\end{document}";

        private const string BadBindingRazorTemplate = @"\begin{document}
  Hello @Model.Batch.Names.
\end{document}";

        private const string BadFormatRazorTemplate = @"\begin{document}
  Hello @Modell.Batch.Names.
\end{document}";

        private const string BadLatexTemplate = @"begin{document}
  Hello @Model.Batch.Name.
\end{document}";

        private static readonly Guid BatchId = Guid.NewGuid();
        private static readonly Guid TemplateId = Guid.NewGuid();
        private static readonly Guid BadBindingRazorTemplateId = Guid.NewGuid();
        private static readonly Guid BadFormatRazorTemplateId = Guid.NewGuid();
        private static readonly Guid BadLatexTemplateId = Guid.NewGuid();

        private readonly IRepository<Batch> _batches = new MemoryRepository<Batch>(new Batch { Id = BatchId, Name = "yada"});
        private readonly IRepository<Template> _templates = new MemoryRepository<Template>(
            new Template { Id = TemplateId, Source = Template },
            new Template { Id = BadBindingRazorTemplateId, Source = BadBindingRazorTemplate },
            new Template { Id = BadFormatRazorTemplateId, Source = BadFormatRazorTemplate },
            new Template { Id = BadLatexTemplateId, Source = BadLatexTemplate });
            
        [Test]
        public void should_render_razor_template()
        {
            var renderService = new TemplateRenderingService(_batches, _templates, new LatexEngine(), new RazorEngine());
            try
            {
                var result = renderService.RenderLatex(TemplateId, BatchId);
                result.ShouldEqual(@"\begin{document}
  Hello yada.
\end{document}");
            }
            catch (TemplateRenderingService.RenderException exception)
            {
                Debug.WriteLine(exception.Details);
                throw;
            }
        }

        [Test]
        public void should_throw_render_exception_when_razor_template_malformed()
        {
            var renderService = new TemplateRenderingService(_batches, _templates, new LatexEngine(), new RazorEngine());
            Assert.Throws<TemplateRenderingService.RenderException>(() => renderService.RenderLatex(BadFormatRazorTemplateId, BatchId));
        }

        [Test]
        public void should_throw_render_exception_when_razor_binding_error()
        {
            var renderService = new TemplateRenderingService(_batches, _templates, new LatexEngine(), new RazorEngine());
            Assert.Throws<TemplateRenderingService.RenderException>(() => renderService.RenderLatex(BadBindingRazorTemplateId, BatchId));
        }

        [Test]
        public void should_render_latex_pdf()
        {
            var renderService = new TemplateRenderingService(_batches, _templates, new LatexEngine(), new RazorEngine());
            string path;
            try
            {
                path = renderService.RenderPdf(TemplateId, BatchId);
            }
            catch (TemplateRenderingService.RenderException exception)
            {
                Debug.WriteLine(exception.Details);
                throw;
            }
            Debug.WriteLine(path);
            File.Exists(path).ShouldBeTrue();
            new FileInfo(path).Length.ShouldNotEqual(0);
            Directory.Delete(Path.GetDirectoryName(path), true);
        }

        [Test]
        public void should_throw_render_exception_when_latex_malformed()
        {
            var renderService = new TemplateRenderingService(_batches, _templates, new LatexEngine(), new RazorEngine());
            Assert.Throws<TemplateRenderingService.RenderException>(() => renderService.RenderPdf(BadLatexTemplateId, BatchId));
        }
    }
}