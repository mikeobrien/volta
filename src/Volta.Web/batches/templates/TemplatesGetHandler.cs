using System.Collections.Generic;
using System.Linq;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Templates
{
    public class TemplatesGetRequest
    {
        public int Index { get; set; }
    }

    public class TemplatesGetHandler
    {
        private readonly IRepository<Template> _templates;
        public const int PageSize = 20;

        public TemplatesGetHandler(IRepository<Template> templates)
        {
            _templates = templates;
        }

        public List<TemplateModel> Execute(TemplatesGetRequest request)
        {
            IQueryable<Template> templates = _templates.OrderBy(x => x.Name);
            if (request.Index > 0) templates = templates.Page(request.Index, PageSize);
            return templates.Select(x => new TemplateModel {
                                    id = x.Id, 
                                    name = x.Name, 
                                    createdBy = x.CreatedBy, 
                                    created = x.Created
                                }).ToList();
        }
    }
}