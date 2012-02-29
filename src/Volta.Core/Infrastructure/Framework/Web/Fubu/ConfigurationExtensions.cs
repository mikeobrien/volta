using System.Linq;
using FubuMVC.Core.Registration.DSL;

namespace Volta.Core.Infrastructure.Framework.Web.Fubu
{
    public static class ConfigurationExtensions
    {
        public static ActionCallCandidateExpression IncludeTypeNamesSuffixed(this ActionCallCandidateExpression expression, params string[] suffix)
        {
            suffix.ToList().ForEach(x => expression.IncludeTypes(y => y.Name.EndsWith(x)));
            return expression;
        }

        public static ActionCallCandidateExpression IncludeMethodsPrefixed(this ActionCallCandidateExpression expression, params string[] prefix)
        {
            prefix.ToList().ForEach(x => expression.IncludeMethods(y => y.Name.StartsWith(x)));
            return expression;
        }
    }
}
