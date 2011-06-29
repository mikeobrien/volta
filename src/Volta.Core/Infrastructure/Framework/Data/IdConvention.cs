namespace Volta.Core.Infrastructure.Framework.Data
{
    public class IdConvention : IIdentityConvention
    {
        public object GetIdentityTemplate(object id)
        {
            return new { Id = id };
        }

        public object GetIdentity(object entity)
        {
            return entity.ToDynamic().Id;
        }
    }
}