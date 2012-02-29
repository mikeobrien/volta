using System;
using System.Linq.Expressions;
using FubuMVC.Core.UI;
using FubuMVC.Core.View;
using HtmlTags;

namespace Volta.Core.Infrastructure.Framework.Web.Fubu
{
    public static class PageExtensions
    {
        public static HtmlTag HiddenInputFor<TInputModel>(this IFubuPage<TInputModel> page, Expression<Func<TInputModel, object>> expression) where TInputModel : class
        {
            return page.Tags().InputFor(expression).Attr("type", "hidden");
        }

        public static FormTag FormAndIdFor<TInputModel>(this IFubuPage page) where TInputModel : class, new()
        {
            return (FormTag)page.FormFor<TInputModel>().Id(typeof(TInputModel).Name);
        }

        public static HtmlTag SubmitFor<TInputModel>(this IFubuPage page, Expression<Func<TInputModel, object>> property, object text) 
            where TInputModel : class, new()
        {
            var name = property.GetPropertyName();
            return new HtmlTag("input").
                Id(name).
                Attr("name", name).
                Attr("value", text).
                Attr("type", "submit");
        }
    }
}