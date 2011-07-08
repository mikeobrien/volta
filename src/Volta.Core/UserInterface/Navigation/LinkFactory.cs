using System.Collections.Generic;

namespace Volta.Core.UserInterface.Navigation
{
    public class LinkFactory : ILinkFactory
    {
        public IEnumerable<Link> Build()
        {
            return new List<Link> { new Link { Name = "Test Batches", Visible = true, Order = 0, Url = "/testbatches/query" },
                                    new Link { Name = "Administration", Order = 1, Visible = true, Children =
                                             {
                                                 new Link { Name = "Users", Order = 0, Visible = true, Url = "/administration/users/query" },
                                                 new Link { Name = "Add User", Order = 1, Visible = true, Url = "/administration/users/add" },
                                                 new Link { Visible = false, Url = "/administration/users/edit" }
                                             }}};
        }
    }
}
