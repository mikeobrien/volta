using System.Collections.Generic;
using System.Linq;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Batches.Schedules
{
    public class SchedulesGetRequest
    {
        public int Index { get; set; }
    }

    public class SchedulesGetHandler
    {
        private readonly IRepository<ScheduleFile> _schedules;
        public const int PageSize = 20;

        public SchedulesGetHandler(IRepository<ScheduleFile> schedules)
        {
            _schedules = schedules;
        }

        public List<ScheduleModel> Execute(SchedulesGetRequest request)
        {
            return _schedules.OrderBy(x => x.Name).Page(request.Index, PageSize).
                Select(x => new ScheduleModel {
                                    id = x.Id, 
                                    name = x.Name, 
                                    createdBy = x.CreatedBy, 
                                    created = x.Created
                                }).ToList();
        }
    }
}