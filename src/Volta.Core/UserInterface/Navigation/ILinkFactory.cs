using System.Collections.Generic;

namespace Volta.Core.UserInterface.Navigation
{
    public interface ILinkFactory
    {
        IEnumerable<Link> Build();
    }
}