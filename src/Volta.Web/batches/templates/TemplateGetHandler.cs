using System;
using AutoMapper;
using Volta.Core.Domain;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Templates
{
    public class TemplateGetHandler
    {
        private readonly IRepository<Template> _templates;

        public TemplateGetHandler(IRepository<Template> templates)
        {
            _templates = templates;
        }

        public TemplateModel Execute_id(TemplateModel request)
        {
            var template = _templates.Get(request.id);
            if (template == null) throw new NotFoundException("Template");
            return Mapper.Map<TemplateModel>(template);
        }
    }
}