using System.Linq;
using System.Reflection;
using FubuMVC.Core.Registration.DSL;

namespace Volta.Core.Infrastructure.Web
{
public static class FubuConfigurationExtensions
{
    public static ActionCallCandidateExpression IncludeTypeNameSuffix(this ActionCallCandidateExpression expression, string methodNameSuffix)
    {
        return expression.IncludeTypesNamed(x => x.EndsWith(methodNameSuffix));
    }

    public static ActionCallCandidateExpression IncludeMethodsNamed(this ActionCallCandidateExpression expression, params string[] methodNames)
    {
        methodNames.ToList().ForEach(x => expression.IncludeMethods(y => y.Method.Name == x));
        return expression;
    }

    public static RouteConventionExpression IgnoreMethodsNamed(this RouteConventionExpression expression, params string[] methodNames)
    {
        methodNames.ToList().ForEach(x => expression.IgnoreMethodsNamed(x));
        return expression;
    }

    public static RouteConventionExpression ConstrainMethodNameToHttpGetMethod(this RouteConventionExpression expression, string methodName)
    {
        return expression.ConstrainToHttpMethod(x => x.Method.Name == methodName, "GET");
    }

    public static RouteConventionExpression ConstrainMethodNameToHttpPostMethod(this RouteConventionExpression expression, string methodName)
    {
        return expression.ConstrainToHttpMethod(x => x.Method.Name == methodName, "POST");
    }

    public static RouteConventionExpression IgnoreExecutingAssemblyNamespaceText(this RouteConventionExpression expression, string namespaceText)
    {
        return expression.IgnoreNamespaceText(string.Format("{0}.{1}", Assembly.GetCallingAssembly().GetName().Name, namespaceText));
    }
}
}