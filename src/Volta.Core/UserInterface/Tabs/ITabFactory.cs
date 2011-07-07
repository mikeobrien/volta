using System.Collections.Generic;

namespace Volta.Core.UserInterface.Tabs
{
    public interface ITabFactory
    {
        IEnumerable<Tab> Build();
    }
}