using System;
using System.Linq;
using System.Linq.Expressions;
using FubuMVC.Core;
using FubuMVC.Core.UI;
using FubuMVC.Core.View;
using HtmlTags;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public static class FubuPageExtensions
    {
        public static HtmlTag DisplayFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression, string className)
            where T : class
        {
            return page.Tags().DisplayFor(expression).AddClass(className);
        }

        public static HtmlTag LabelFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression, string className)
               where T : class
        {
            return page.Tags().LabelFor(expression).AddClass(className);
        }

        public static HtmlTag InputFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression, string className)
            where T : class
        {
            return page.Tags().InputFor(expression).AddClass(className);
        }

        public static HtmlTag HiddenInputFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression) where T : class
        {
            return page.Tags().InputFor(expression).Attr("type", "hidden");
        }

        public static FormTag StartForm<TInputModel>(this IFubuPage page) where TInputModel : class, new()
        {
            return (FormTag)page.FormFor<TInputModel>().Id(typeof(TInputModel).Name);
        }

        public static HtmlTag SubmitFor<TInputModel>(this IFubuPage page, Expression<Func<TInputModel, object>> property, string text, string className) 
            where TInputModel : class, new()
        {
            var name = property.GetPropertyName();
            return new HtmlTag("input").
                Id(name).
                Attr("name", name).
                AddClass(className).
                Attr("value", text).
                Attr("type", "submit");
        }

        //public static HtmlTag LinkTo<TInputModel>(this IFubuPage page, params Expression<Func<TInputModel, object>>[] querystring) where TInputModel : class, new()
        //{
        //    var tag = FubuMVC.Core.UI.FubuPageExtensions.LinkTo<TInputModel>(page);
        //    if (querystring.Any()) tag.Attr("href", string.Format("{0}?{1}", tag.Attr("href"), querystring.Select(x => string.Format("{0}={1}", x.))));
        //    return tag;
        //}

    }
}