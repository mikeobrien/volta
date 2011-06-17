namespace Volta.Core.Infrastructure.Data
{
    public interface IIdentityConvention
    {
        object GetIdentityTemplate(object id);
        object GetIdentity(object entity);
    }
}