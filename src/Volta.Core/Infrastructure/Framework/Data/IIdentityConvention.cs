namespace Volta.Core.Infrastructure.Framework.Data
{
    public interface IIdentityConvention
    {
        object GetIdentityTemplate(object id);
        object GetIdentity(object entity);
    }
}