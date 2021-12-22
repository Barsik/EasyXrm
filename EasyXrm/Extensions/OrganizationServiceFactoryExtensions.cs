using System;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Extensions
{
    public static class OrganizationServiceFactoryExtensions
    {
        public static Lazy<IOrganizationService> CreateLazyOrganizationService(
            this IOrganizationServiceFactory organizationServiceFactory, Guid? userId) =>
            new Lazy<IOrganizationService>(() =>
                organizationServiceFactory.CreateOrganizationService(userId));
    }
}
