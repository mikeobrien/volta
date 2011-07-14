using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Web.Handlers.Administration.Users
{
    public class QueryPagedModel
    {
        public int Page { get; set; }
    }

    public class QueryOutputModel
    {
        public IEnumerable<User> Users { get; set; }
        public PagingModel Paging { get; set; }

        public class User
        {
            public string Username { get; set; }
            public bool Administrator { get; set; }
        }
    }

    public class QueryHandler
    {
        private readonly IRepository<User> _users;

        public QueryHandler(IRepository<User> users)
        {
            _users = users;
        }

        public QueryOutputModel Query()
        {
            return Query_Page(new QueryPagedModel { Page = 1 });
        }

        public QueryOutputModel Query_Page(QueryPagedModel query)
        {
            const int pageSize = 20;
            var pagedResults = _users.OrderBy(x => x.Username).GetPage(pageSize, query.Page);
            var paging = new PagingModel(pagedResults.TotalRecords, pageSize, query.Page);
            return new QueryOutputModel
                       {
                           Paging = paging,
                           Users = pagedResults.Results.Select(
                                x => new QueryOutputModel.User { Username = x.Username, 
                                                                 Administrator = x.Administrator})
                       };
        }
    }
}