using System;
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

            Assert.Equal(1, trackedOrganizationService.ChangeTracker.TrackedEntities.Count());
        }

        [Fact]
        public void ChangeTrackedOrganizationService_UpdateSuccessfully()
        {
            var context = new XrmFakedContext();

            var organizationService = context.GetOrganizationService();

            var trackedOrganizationService = new ChangeTrackedOrganizationService(organizationService);
            var test = new Entity("test",Guid.NewGuid());
            test["t0"] = "t0";
            trackedOrganizationService.Create(test);
            test["t1"] = "t1";
            test["t2"] = "t2";

            Assert.Equal(3, test.Attributes.Count);
            trackedOrganizationService.Update(test);

            Assert.Equal(2, test.Attributes.Count);
        }

        [Fact]
        public void ChangeTrackedOrganizationService_DeleteSuccessfully()
        {
            var context = new XrmFakedContext();
            var organizationService = context.GetOrganizationService();
            var trackedOrganizationService = new ChangeTrackedOrganizationService(organizationService);
            var testId = Guid.NewGuid();

            var test = new Entity("test", testId);
            trackedOrganizationService.Create(test);
            trackedOrganizationService.Delete("test", testId);

            Assert.Equal(0, trackedOrganizationService.ChangeTracker.TrackedEntities.Count());
        }


        [Fact]
        public void ChangeTrackedOrganizationService_DeleteNonTracked()
        {
            var context = new XrmFakedContext();
            var organizationService = context.GetOrganizationService();
            var trackedOrganizationService = new ChangeTrackedOrganizationService(organizationService);
            var testId = Guid.NewGuid();

            var test = new Entity("test", testId);
            organizationService.Create(test);
            trackedOrganizationService.Delete("test", testId);

            Assert.Equal(0, trackedOrganizationService.ChangeTracker.TrackedEntities.Count());
        }
    }
}