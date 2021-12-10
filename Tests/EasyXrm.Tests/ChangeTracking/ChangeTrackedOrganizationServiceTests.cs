using System.Linq;
using EasyXrm.ChangeTracking;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace EasyXrm.Tests.ChangeTracking
{
    public class ChangeTrackedOrganizationServiceTests
    {
        [Fact]
        public void ChangeTrackedOrganizationService_CreateSuccessfully()
        {
            var context = new XrmFakedContext();

            var organizationService = context.GetOrganizationService();

            var trackedOrganizationService = new ChangeTrackedOrganizationService(organizationService);

            trackedOrganizationService.Create(new Entity("test"));

            Assert.Equal(1,trackedOrganizationService.ChangeTracker.TrackedEntities.Count());
        }
    }
}
