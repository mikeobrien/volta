using System.Collections.Generic;
using System.Linq;

namespace Volta.Web.Handlers.Administration.Users
{
    public class QueryOutputModel
    {
        public IEnumerable<User> Users { get; set; }
        public int PageStart { get; set; }
        public int PageEnd { get; set; }
        public int Total { get; set; }
        public int PreviousPage { get; set; }
        public int NextPage { get; set; }

        public class User
        {
            public string Username { get; set; }
            public bool Administrator { get; set; }
        }
    }

    public class QueryHandler
    {
        public QueryOutputModel Query()
        {
            return new QueryOutputModel
                       {
                           PageStart = 1,
                           PageEnd = 10,
                           Total = 50,
                           PreviousPage = 0,
                           NextPage = 2,
                           Users = Enumerable.Range(1, 10).Select(x => new QueryOutputModel.User { Username = "user" + x, Administrator = (x % 2 == 0)})
                       };
        }
    }
}