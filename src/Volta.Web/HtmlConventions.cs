using FubuMVC.Core.UI;

namespace Volta.Web
{
    public class HtmlConventions : HtmlConventionRegistry
    {
        public HtmlConventions()
        {
            Editors.Always.Modify((r, t) => t.Id(r.ElementId));
            Labels.Always.Modify((r, t) => t.Id(r.ElementId));
            Displays.Always.Modify((r, t) => t.Id(r.ElementId));
        }
    }
}