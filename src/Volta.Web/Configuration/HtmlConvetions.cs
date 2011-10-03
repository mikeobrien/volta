using FubuMVC.Core.UI;
using HtmlTags;

namespace Volta.Web.Configuration
{
    public class VoltaHtmlConventions : HtmlConventionRegistry
    {
        public VoltaHtmlConventions()
        {
            Displays.
                Always.Modify((r, t) => t.Id(r.ElementId));

            Labels
                .Always.Modify((r, t) => t.Attr("for", r.ElementId).Id(r.ElementId + "-label").AddClass("formlabel"));

            Editors.Always.Modify((r, t) => t.Id(r.ElementId).AddClass("textbox"));

            Editors
                .If(accessor => accessor.Accessor.FieldName.ToLowerInvariant().Contains("password"))
                .Modify(tag => tag.Attr("type", "password"));

            Editors
                .If(accessor => accessor.Accessor.FieldName.ToLowerInvariant().Contains("description"))
                .BuildBy(x => new HtmlTag("textarea"));
        }
    }
}