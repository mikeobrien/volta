using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Templates
{
    public class TemplateDeleteHandler
    {
        private readonly IRepository<Template> _templates;

        public TemplateDeleteHandler(IRepository<Template> templates)
        {
            _templates = templates;
        }

        public void Execute_id(TemplateModel request)
        {
            _templates.Delete(request.id);
        }
    }
}