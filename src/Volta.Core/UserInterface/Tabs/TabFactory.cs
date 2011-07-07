using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Volta.Core.UserInterface.Tabs
{
    public class TabFactory : ITabFactory
    {
        public IEnumerable<Tab> Build()
        {
            return new List<Tab> { new Tab {Name = "Test Batches", Order = 0, Url = "/testbatches"},
                                   new Tab {Name = "Administration", Order = 1, Tabs =
                                            {
                                                new Tab {Name = "Users", Order = 0, Url = "/administration/users/query"},
                                                new Tab {Name = "Add User", Order = 1, Url = "/administration/users/add"}
                                            }}};
        }
    }
}
