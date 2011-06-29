using Norm;
using Norm.Collections;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public static class IMongoCollectionExtensions
    {
        public static T FindOneByIdConvention<T>(this IMongoCollection<T> collection, object id, IIdentityConvention identityConvention)
        {
            return collection.FindOne(identityConvention.GetIdentityTemplate(id));
        }

        public static void DeleteByIdConvention<T>(this IMongoCollection<T> collection, object id, IIdentityConvention identityConvention)
        {
            collection.Delete(identityConvention.GetIdentityTemplate(id));
        }

        public static void UpdateOneByIdConvention<T>(this IMongoCollection<T> collection, T valueDocument, IIdentityConvention identityConvention)
        {
            collection.UpdateOne(identityConvention.GetIdentityTemplate(identityConvention.GetIdentity(valueDocument)), valueDocument);
        }
    }
}