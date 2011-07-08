using System;

namespace Volta.Core.UserInterface.Navigation
{
    public enum Module
    {
        TestBatches,
        Administration
    }

    public class NavigationAttribute : Attribute
    {
        public NavigationAttribute(Module module)
        {
            Module = module;
            Visible = false;
        }

        public NavigationAttribute(Module module, string name, int order)
        {
            Module = module;
            Name = name;
            Order = order;
            Visible = true;
        }

        public Module Module { get; private set; }
        public string Name { get; private set; }
        public int Order { get; private set; }
        public bool Visible { get; private set; }
    }
}