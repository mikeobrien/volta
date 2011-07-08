using System.Collections.Generic;
using System.Linq;

namespace Volta.Core.UserInterface.Navigation
{
    public class Link
    {
        public Link()
        {
            Children = new List<Link>();
        }

        public string Name { get; set; }
        public int Order { get; set; }
        public bool Visible { get; set; }
        public string Url { get; set; }
        public bool HasUrl { get { return !string.IsNullOrEmpty(Url); } }
        public IList<Link> Children { get; private set; }
        public bool HasChildren { get { return Children.Any(); } }
    }
}