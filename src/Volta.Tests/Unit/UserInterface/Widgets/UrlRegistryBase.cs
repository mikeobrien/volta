using System;
using System.Linq.Expressions;
using System.Reflection;
using FubuMVC.Core.Assets.Files;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.Urls;

namespace Volta.Tests.Unit.UserInterface.Widgets
{
    public abstract class UrlRegistryBase : IUrlRegistry
    {
        public virtual string UrlFor(object model) { throw new NotImplementedException(); }
        public virtual string UrlFor(object model, string category) { throw new NotImplementedException(); }
        public virtual string UrlFor<T>(string categoryOrHttpMethod = null) where T : class { throw new NotImplementedException(); }
        public virtual string UrlFor(Type handlerType, MethodInfo method = null, string categoryOrHttpMethodOrHttpMethod = null) { throw new NotImplementedException(); }
        public virtual string UrlFor<TController>(Expression<Action<TController>> expression, string categoryOrHttpMethod = null) { throw new NotImplementedException(); }
        public virtual string UrlForNew<T>() { throw new NotImplementedException(); }
        public virtual string UrlForNew(Type entityType) { throw new NotImplementedException(); }
        public virtual bool HasNewUrl<T>() { throw new NotImplementedException(); }
        public virtual bool HasNewUrl(Type type) { throw new NotImplementedException(); }
        public virtual string TemplateFor(object model, string categoryOrHttpMethod = null) { throw new NotImplementedException(); }
        public virtual string UrlForPropertyUpdate(object model) { throw new NotImplementedException(); }
        public virtual string UrlForPropertyUpdate(Type type) { throw new NotImplementedException(); }
        public virtual string TemplateFor<TModel>(params Func<object, object>[] hash) where TModel : class, new() { throw new NotImplementedException(); }
        public virtual string UrlFor(Type modelType, RouteParameters parameters, string categoryOrHttpMethod = null) { throw new NotImplementedException(); }
        public virtual string UrlForAsset(AssetFolder? folder, string name) { throw new NotImplementedException(); }
        public virtual string UrlFor(Type modelType, string category, RouteParameters parameters) { throw new NotImplementedException(); }
    }
}
