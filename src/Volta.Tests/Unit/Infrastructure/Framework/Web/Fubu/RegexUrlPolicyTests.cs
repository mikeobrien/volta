using System;
using System.Linq.Expressions;
using System.Reflection;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Web.Fubu;
using Volta.Tests.Unit.Infrastructure.Framework.Web.Fubu.Web.Handlers;

namespace Volta.Tests.Unit.Infrastructure.Framework.Web.Fubu
{
    namespace Web.Handlers
    {
        public class GetHandler { public object Execute() { return null; } }
        public class InputModel { public int Id { get; set; } public string Name { get; set; } }
        public class GetByIdAndNameHandler { public object Execute_Id_Name(InputModel model) { return null; } }
        public class PostHandler { public object Execute() { return null; } }
        public class PutHandler { public object ExecutePut() { return null; } }
        public class DeleteHandler { public object ExecuteDelete() { return null; } }
        public class GetUserHandler { public object Execute() { return null; } }
        public class PostUserHandler { public object Execute() { return null; } }
        public class PutUserHandler { public object Execute() { return null; } }
        public class DeleteUserHandler { public object Execute() { return null; } }
        public class UserGetHandler { public object Execute() { return null; } }
        public class UserPostHandler { public object Execute() { return null; } }
        public class UserPutHandler { public object Execute() { return null; } }
        public class UserDeleteHandler { public object Execute() { return null; } }
    }

    [TestFixture]
    public class RegexUrlPolicyTests
    {
        private readonly static string Namespace = typeof(RegexUrlPolicyTests).Namespace.ToLower().Replace('.', '/');
        private static readonly string RelativeNamespace =
            Namespace.Replace(Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace('.', '/') + "/", string.Empty);

        [Test]
        public void default_should_include_namespace_class_and_method_for_all_http_methods()
        {
            var route = CreateRoute<GetHandler>(RegexUrlPolicy.Create(), x => x.Execute());
            route.AllowedHttpMethods.ShouldBeEmpty();
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler/execute");
        }

        [Test]
        public void should_ignore_entire_namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespace();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual("gethandler/execute");
        }

        [Test]
        public void should_ignore_current_assembly_namespace_of_type()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreAssemblyNamespace(typeof(RegexUrlPolicyTests));
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(RelativeNamespace + "/web/handlers/gethandler/execute");
        }

        [Test]
        public void should_ignore_current_assembly_namespace_of_generic_type()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreAssemblyNamespace<RegexUrlPolicyTests>();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(RelativeNamespace + "/web/handlers/gethandler/execute");
        }

        [Test]
        public void should_ignore_current_assembly()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreAssemblyNamespace<RegexUrlPolicyTests>();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(RelativeNamespace + "/web/handlers/gethandler/execute");
        }

        [Test]
        public void should_ignore_type_namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespace<RegexUrlPolicyTests>();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual("web/handlers/gethandler/execute");
        }

        [Test]
        public void should_ignore_custom_namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespaces("Web.Handlers");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/gethandler/execute");
        }

        [Test]
        public void should_ignore_custom_regex_namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespaces("H.*?s");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/gethandler/execute");
        }

        [Test]
        public void should_ignore_multiple_custom_regex_namespaces()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespaces("H.*?d", "ers");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/l/gethandler/execute");
        }

        [Test]
        public void should_ignore_entire_class_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreClassName();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/execute");
        }

        [Test]
        public void should_ignore_custom_class_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreClassNames("Handler");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/get/execute");
        }

        [Test]
        public void should_ignore_custom_regex_class_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreClassNames("H.*?r");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/get/execute");
        }

        [Test]
        public void should_ignore_multiple_custom_regex_class_names()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreClassNames("H.*?d", "l.*$");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/get/execute");
        }

        [Test]
        public void should_ignore_entire_method_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreMethodName();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler");
        }

        [Test]
        public void should_ignore_custom_method_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreMethodNames("cute");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler/exe");
        }

        [Test]
        public void should_ignore_custom_regex_method_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreMethodNames("c.*$");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler/exe");
        }

        [Test]
        public void should_ignore_multiple_custom_regex_method_names()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreMethodNames("^.*?e", "u.*$");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler/c");
        }

        [Test]
        public void should_include_url_parameters()
        {
            var policy = RegexUrlPolicy.Create();
            var route = CreateRoute<GetByIdAndNameHandler>(policy, x => x.Execute_Id_Name(null));
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/getbyidandnamehandler/execute/{Id}/{Name}");
        }

        [Test]
        public void should_constrain_to_get_by_namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainNamespaceToHttpGet("Web");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void should_constrain_to_post_by_namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainNamespaceToHttpPost("Web");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void should_constrain_to_put_by_namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainNamespaceToHttpPut("Web");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void should_constrain_to_delete_by_namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainNamespaceToHttpDelete("Web");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        [Test]
        public void should_constrain_to_get_by_class_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpGet("Get");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void should_constrain_to_post_by_class_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPost("Post");
            var route = CreateRoute<PostHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void should_constrain_to_put_by_class_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPut("Put");
            var route = CreateRoute<PutHandler>(policy, x => x.ExecutePut());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void should_constrain_to_delete_by_class_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpDelete("Delete");
            var route = CreateRoute<DeleteHandler>(policy, x => x.ExecuteDelete());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        [Test]
        public void should_constrain_to_get_by_class_name_starting_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpGetStartingWith("Get");
            var route = CreateRoute<GetUserHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void should_constrain_to_post_by_class_name_starting_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPostStartingWith("Post");
            var route = CreateRoute<PostUserHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void should_constrain_to_put_by_class_name_starting_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPutStartingWith("Put");
            var route = CreateRoute<PutUserHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void should_constrain_to_delete_by_class_name_starting_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpDeleteStartingWith("Delete");
            var route = CreateRoute<DeleteUserHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        [Test]
        public void should_constrain_to_get_by_class_name_ending_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpGetEndingWith("GetHandler");
            var route = CreateRoute<UserGetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void should_constrain_to_post_by_class_name_ending_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPostEndingWith("PostHandler");
            var route = CreateRoute<UserPostHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void should_constrain_to_put_by_class_name_ending_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPutEndingWith("PutHandler");
            var route = CreateRoute<UserPutHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void should_constrain_to_delete_by_class_name_ending_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpDeleteEndingWith("DeleteHandler");
            var route = CreateRoute<UserDeleteHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        [Test]
        public void should_constrain_to_get_by_method_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpGet("Execute");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void should_constrain_to_post_by_method_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpPost("Execute");
            var route = CreateRoute<PostHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void should_constrain_to_put_by_method_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpPut("Put");
            var route = CreateRoute<PutHandler>(policy, x => x.ExecutePut());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void should_constrain_to_delete_by_method_name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpDelete("Delete");
            var route = CreateRoute<DeleteHandler>(policy, x => x.ExecuteDelete());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        [Test]
        public void should_constrain_to_get_by_method_name_starting_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpGet("Execute");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void should_constrain_to_post_by_method_name_starting_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpPost("Execute");
            var route = CreateRoute<PostHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void should_constrain_to_put_by_method_name_starting_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpPut("Put");
            var route = CreateRoute<PutHandler>(policy, x => x.ExecutePut());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void should_constrain_to_delete_by_method_name_starting_with()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpDelete("Delete");
            var route = CreateRoute<DeleteHandler>(policy, x => x.ExecuteDelete());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        private IRouteDefinition CreateRoute<T>(IUrlPolicy policy, Expression<Func<T, object>> action)
        {
            var actionCall = new ActionCall(typeof(T), typeof(T).GetMethod(((MethodCallExpression)action.Body).Method.Name));
            return policy.Build(actionCall);
        }
    }
}