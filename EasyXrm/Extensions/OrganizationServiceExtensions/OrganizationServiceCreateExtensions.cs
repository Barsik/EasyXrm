using System;
using EasyXrm.Utilities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Extensions.OrganizationServiceExtensions
{
    public static class OrganizationServiceCreateExtensions
    {
        public static Entity CreateThenSetId(this IOrganizationService organizationService, Entity entity)
        {
            if (organizationService == null) throw new ArgumentNullException(nameof(organizationService));
            entity.Id = organizationService.Create(entity);
            return entity;
        }

        public static Entity CreateThenRetrieve(this IOrganizationService organizationService, Entity entity,
            ColumnSet columnSet)
        {
            if (organizationService == null) throw new ArgumentNullException(nameof(organizationService));
            var id = organizationService.Create(entity);
            return organizationService.Retrieve(entity.LogicalName, entity.Id, columnSet);
        }
    }
}