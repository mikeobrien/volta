using System;
using Volta.Core.Domain.Batches;

namespace Volta.Web.Batches
{
    public class PresentationRequest
    {
        public Guid batchId { get; set; }
        public Guid templateId { get; set; }
        public string format { get; set; }
    }

    public class PresentationModel
    {
        public Guid fileId { get; set; }
        public bool error { get; set; }
        public string errorMessage { get; set; }
    }

    public class PresentationPostHandler
    {
        private readonly PresentationService _presentationService;

        public PresentationPostHandler(PresentationService presentationService)
        {
            _presentationService = presentationService;
        }

        public PresentationModel ExecutePresentation(PresentationRequest request)
        {
            try
            {
                var fileId = request.format == "pdf" ? 
                    _presentationService.RenderPdf(request.templateId, request.batchId) : 
                    _presentationService.RenderLatex(request.templateId, request.batchId);
                return new PresentationModel { fileId = fileId };
            }
            catch (PresentationService.RenderException exception)
            {
                return new PresentationModel { error = true, errorMessage = exception.Details };
            }
        }
    }
}