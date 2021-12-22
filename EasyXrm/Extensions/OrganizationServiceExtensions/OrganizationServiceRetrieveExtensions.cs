using System;
using System.Linq.Expressions;
using EasyXrm.Query;
using EasyXrm.Utilities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Extensions.OrganizationServiceExtensions
{
    public static class OrganizationServiceRetrieveExtensions
    {
        public static Entity Retrieve(this IOrganizationService organizationService, string entityName,
            Guid id, params string[] attributes)
        {
            if (organizationService == null) throw new ArgumentNullException(nameof(organizationService));
            return organizationService.Retrieve(entityName, id, new ColumnSet(attributes));
        }

        public static Entity Retrieve(this IOrganizationService organizationService, EntityReference entityReference,
            params string[] attributes)
        {
            if (organizationService == null) throw new ArgumentNullException(nameof(organizationService));
            return organizationService.Retrieve(entityReference.LogicalName, entityReference.Id,
                new ColumnSet(attributes));
        }

        public static TEntity Retrieve<TEntity>(this IOrganizationService organizationService, Guid id,
            params Expression<Func<TEntity, object>>[] attributes) where TEntity : Entity
        {
            if (organizationService == null) throw new ArgumentNullException(nameof(organizationService));

            return organizationService.Retrieve(LogicalName.GetName<TEntity>(), id, ColumnSet<TEntity>.With(attributes))
                .ToEntity<TEntity>();
        }
    }
}

