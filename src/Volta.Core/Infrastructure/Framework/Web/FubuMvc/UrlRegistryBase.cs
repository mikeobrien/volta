using System;
using System.Linq.Expressions;
using System.Reflection;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.Urls;

namespace Volta.Core.Infrastructure.Framework.Web.FubuMvc
{
    public abstract class UrlRegistryBase : IUrlRegistry
    {
        public virtual string UrlFor(object model) { throw new NotImplementedException(); }
        public virtual string UrlFor(object model, string category) { throw new NotImplementedException(); }
        public virtual string UrlFor<TController>(Expression<Action<TController>> expression) { throw new NotImplementedException(); }
        public virtual string UrlForNew<T>() { throw new NotImplementedException(); }
        public virtual string UrlForNew(Type entityType) { throw new NotImplementedException(); }
        public virtual bool HasNewUrl<T>() { throw new NotImplementedException(); }
        public virtual bool HasNewUrl(Type type) { throw new NotImplementedException(); }
        public virtual string UrlForPropertyUpdate(object model) { throw new NotImplementedException(); }
        public virtual string UrlForPropertyUpdate(Type type) { throw new NotImplementedException(); }
        public virtual string UrlFor(Type handlerType, MethodInfo method) { throw new NotImplementedException(); }
        public virtual string TemplateFor(object model) { throw new NotImplementedException(); }
        public string TemplateFor<TModel>(params Func<object, object>[] hash) where TModel : class, new() { throw new NotImplementedException(); }
        public virtual string UrlFor(Type modelType, RouteParameters parameters) { throw new NotImplementedException(); }
        public virtual string UrlFor(Type modelType, string category, RouteParameters parameters) { throw new NotImplementedException(); }
    }
}