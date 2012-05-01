using AutoMapper;
using Volta.Core.Domain.Administration;
using Volta.Core.Domain.Batches;
using Volta.Core.Infrastructure.Framework.AutoMapper;
using Volta.Web.Admin.Users;
using Volta.Web.Batches;
using Volta.Web.Batches.Schedules;
using Volta.Web.Batches.Templates;

namespace Volta.Web
{
    public class MappingConventions
    {
        public static void Register()
        {
            Mapper.CreateMap<User, UserModel>().ForMember(x => x.password, x => x.Ignore()).
                ToBidirectional().ForMember(x => x.PasswordHash, x => x.Ignore());
            Mapper.CreateMap<ScheduleFile, ScheduleModel>().ToBidirectional();
            Mapper.CreateMap<Batch, BatchModel>().ToBidirectional();
            Mapper.CreateMap<Template, TemplateModel>().ToBidirectional();
        }
    }
}