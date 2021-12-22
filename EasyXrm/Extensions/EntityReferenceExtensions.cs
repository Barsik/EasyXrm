using Microsoft.Xrm.Sdk;

namespace EasyXrm.Extensions
{
    public static class EntityReferenceExtensions
    {
        public static Entity ToEntity(this EntityReference entityReference) =>
            new Entity(entityReference.LogicalName, entityReference.Id);

        public static TEntity ToEntity<TEntity>(this EntityReference entityReference) where TEntity : Entity =>
            entityReference.ToEntity().ToEntity<TEntity>();
    }
}