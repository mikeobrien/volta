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
    }

    [TestFixture]
    public class RegexUrlPolicyTests
    {
        private readonly static string Namespace = typeof(RegexUrlPolicyTests).Namespace.ToLower().Replace('.', '/');
        private static readonly string RelativeNamespace =
            Namespace.Replace(Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace('.', '/') + "/", string.Empty);

        [Test]
        public void Default_Should_Include_Namespace_Class_And_Method_For_All_Http_Methods()
        {
            var route = CreateRoute<GetHandler>(RegexUrlPolicy.Create(), x => x.Execute());
            route.AllowedHttpMethods.ShouldBeEmpty();
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler/execute");
        }

        [Test]
        public void Should_Ignore_Entire_Namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespace();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual("gethandler/execute");
        }

        [Test]
        public void Should_Ignore_Current_Assembly_Namespace_Of_Type()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreAssemblyNamespace(typeof(RegexUrlPolicyTests));
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(RelativeNamespace + "/web/handlers/gethandler/execute");
        }

        [Test]
        public void Should_Ignore_Current_Assembly_Namespace_Of_Generic_Type()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreAssemblyNamespace<RegexUrlPolicyTests>();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(RelativeNamespace + "/web/handlers/gethandler/execute");
        }

        [Test]
        public void Should_Ignore_Current_Assembly()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreAssemblyNamespace<RegexUrlPolicyTests>();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(RelativeNamespace + "/web/gethandler/execute");
        }

        [Test]
        public void Should_Ignore_Type_Namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespace<RegexUrlPolicyTests>();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual("web/handlers/gethandler/execute");
        }

        [Test]
        public void Should_Ignore_Custom_Namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespaces("Web.Handlers");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/gethandler/execute");
        }

        [Test]
        public void Should_Ignore_Custom_Regex_Namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespaces("H.*?s");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/gethandler/execute");
        }

        [Test]
        public void Should_Ignore_Multiple_Custom_Regex_Namespaces()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreNamespaces("H.*?d", "ers");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/l/gethandler/execute");
        }

        [Test]
        public void Should_Ignore_Entire_Class_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreClassName();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/execute");
        }

        [Test]
        public void Should_Ignore_Custom_Class_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreClassNames("Handler");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/get/execute");
        }

        [Test]
        public void Should_Ignore_Custom_Regex_Class_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreClassNames("H.*?r");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/get/execute");
        }

        [Test]
        public void Should_Ignore_Multiple_Custom_Regex_Class_Names()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreClassNames("H.*?d", "l.*$");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/get/execute");
        }

        [Test]
        public void Should_Ignore_Entire_Method_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreMethodName();
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler");
        }

        [Test]
        public void Should_Ignore_Custom_Method_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreMethodNames("cute");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler/exe");
        }

        [Test]
        public void Should_Ignore_Custom_Regex_Method_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreMethodNames("c.*$");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler/exe");
        }

        [Test]
        public void Should_Ignore_Multiple_Custom_Regex_Method_Names()
        {
            var policy = RegexUrlPolicy.Create();
            policy.IgnoreMethodNames("^.*?e", "u.*$");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/gethandler/c");
        }

        [Test]
        public void Should_Include_Url_Parameters()
        {
            var policy = RegexUrlPolicy.Create();
            var route = CreateRoute<GetByIdAndNameHandler>(policy, x => x.Execute_Id_Name(null));
            route.Pattern.ShouldEqual(Namespace + "/web/handlers/getbyidandnamehandler/execute/{Id}/{Name}");
        }

        [Test]
        public void Should_Constrain_To_Get_By_Namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainNamespaceToHttpGet("Web");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void Should_Constrain_To_Post_By_Namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainNamespaceToHttpPost("Web");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void Should_Constrain_To_Put_By_Namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainNamespaceToHttpPut("Web");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void Should_Constrain_To_Delete_By_Namespace()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainNamespaceToHttpDelete("Web");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        [Test]
        public void Should_Constrain_To_Get_By_Class_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpGet("Get");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void Should_Constrain_To_Post_By_Class_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPost("Post");
            var route = CreateRoute<PostHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void Should_Constrain_To_Put_By_Class_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPut("Put");
            var route = CreateRoute<PutHandler>(policy, x => x.ExecutePut());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void Should_Constrain_To_Delete_By_Class_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpDelete("Delete");
            var route = CreateRoute<DeleteHandler>(policy, x => x.ExecuteDelete());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        [Test]
        public void Should_Constrain_To_Get_By_Class_Name_Starting_With()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpGetStartingWith("Get");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void Should_Constrain_To_Post_By_Class_Name_Starting_With()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPostStartingWith("Post");
            var route = CreateRoute<PostHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void Should_Constrain_To_Put_By_Class_Name_Starting_With()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpPutStartingWith("Put");
            var route = CreateRoute<PutHandler>(policy, x => x.ExecutePut());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void Should_Constrain_To_Delete_By_Class_Name_Starting_With()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainClassToHttpDeleteStartingWith("Delete");
            var route = CreateRoute<DeleteHandler>(policy, x => x.ExecuteDelete());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        [Test]
        public void Should_Constrain_To_Get_By_Method_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpGet("Execute");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void Should_Constrain_To_Post_By_Method_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpPost("Execute");
            var route = CreateRoute<PostHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void Should_Constrain_To_Put_By_Method_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpPut("Put");
            var route = CreateRoute<PutHandler>(policy, x => x.ExecutePut());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void Should_Constrain_To_Delete_By_Method_Name()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpDelete("Delete");
            var route = CreateRoute<DeleteHandler>(policy, x => x.ExecuteDelete());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("DELETE");
        }

        [Test]
        public void Should_Constrain_To_Get_By_Method_Name_Starting_With()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpGet("Execute");
            var route = CreateRoute<GetHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("GET");
        }

        [Test]
        public void Should_Constrain_To_Post_By_Method_Name_Starting_With()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpPost("Execute");
            var route = CreateRoute<PostHandler>(policy, x => x.Execute());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("POST");
        }

        [Test]
        public void Should_Constrain_To_Put_By_Method_Name_Starting_With()
        {
            var policy = RegexUrlPolicy.Create();
            policy.ConstrainMethodToHttpPut("Put");
            var route = CreateRoute<PutHandler>(policy, x => x.ExecutePut());
            route.AllowedHttpMethods.Count.ShouldEqual(1);
            route.AllowedHttpMethods.ShouldContain("PUT");
        }

        [Test]
        public void Should_Constrain_To_Delete_By_Method_Name_Starting_With()
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