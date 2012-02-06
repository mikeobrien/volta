using System.Linq;
using FubuMVC.Core.Registration.DSL;

namespace Volta.Core.Infrastructure.Framework.Web.FubuMvc
{
    public static class ConfigurationExtensions
    {
        public static ActionCallCandidateExpression IncludeTypeNameSuffix(this ActionCallCandidateExpression expression, string methodNameSuffix)
        {
            return expression.IncludeTypesNamed(x => x.EndsWith(methodNameSuffix));
        }

        public static ActionCallCandidateExpression IncludeMethodsPrefixed(this ActionCallCandidateExpression expression, params string[] prefix)
        {
            prefix.ToList().ForEach(x => expression.IncludeMethods(y => y.Name.StartsWith(x)));
            return expression;
        }

        public static RouteConventionExpression ConstrainMethodPrefixToHttpGet(this RouteConventionExpression expression, string suffix)
        {
            return expression.ConstrainToHttpMethod(x => x.Method.Name.StartsWith(suffix), "GET");
        }

        public static RouteConventionExpression ConstrainMethodPrefixToHttpPost(this RouteConventionExpression expression, string suffix)
        {
            return expression.ConstrainToHttpMethod(x => x.Method.Name.StartsWith(suffix), "POST");
        }
    }
}