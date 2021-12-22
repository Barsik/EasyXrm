using System;
using EasyXrm.Utilities;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Extensions.OrganizationServiceExtensions
{
    public static class OrganizationServiceDeleteExtensions
    {
        public static void Delete(this IOrganizationService organizationService, EntityReference entityReference)
        {
            if (organizationService == null) throw new ArgumentNullException(nameof(organizationService));
            organizationService.Delete(entityReference.LogicalName, entityReference.Id);
        }

        public static void Delete<TEntity>(this IOrganizationService organizationService, TEntity entity)
            where TEntity : Entity
        {
            if (organizationService == null) throw new ArgumentNullException(nameof(organizationService));
            organizationService.Delete(entity.LogicalName, entity.Id);
        }

        public static void Delete<TEntity>(this IOrganizationService organizationService, Guid id)
            where TEntity : Entity
        {
            if (organizationService == null) throw new ArgumentNullException(nameof(organizationService));
            organizationService.Delete(LogicalName.GetName<TEntity>(), id);

        }
    }
}