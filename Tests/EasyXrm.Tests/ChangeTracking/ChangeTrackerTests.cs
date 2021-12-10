using System;
using System.Linq;
using EasyXrm.ChangeTracking;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace EasyXrm.Tests.ChangeTracking
{
    public class ChangeTrackerTests
    {
        [Fact]
        public void ChangeTracker_WhenAttachEntity()
        {
            var changeTracker = new ChangeTracker();
            changeTracker.Attach(new Entity());

            Assert.Equal(1, changeTracker.TrackedEntities.Count());
            changeTracker.Dispose();
        }

        [Fact]
        public void ChangeTracker_WhenAttachNull()
        {
            var changeTracker = new ChangeTracker();
            Entity nullEntity = null;
            Assert.Throws<ArgumentNullException>(() => { changeTracker.Attach(nullEntity); });
            changeTracker.Dispose();
        }

        [Fact]
        public void ChangeTracker_WhenAttachTwice()
        {
            var changeTracker = new ChangeTracker();
            var entity = new Entity();
            var attachFirstTimeResult = changeTracker.Attach(entity);
            var attachSecondTimeResult = changeTracker.Attach(entity);

            Assert.True(attachFirstTimeResult);
            Assert.False(attachSecondTimeResult);
            Assert.Equal(1, changeTracker.TrackedEntities.Count());
            changeTracker.Dispose();
        }


        [Fact]
        public void ChangeTracker_WhenChangedSomeAttribute()
        {
            var changeTracker = new ChangeTracker();
            var entity = new Entity("test", Guid.NewGuid());
            entity.Attributes.Add("test_attribute1", "Hello");
            entity.Attributes.Add("test_attribute2", "Hell");
            var attachFirstTimeResult = changeTracker.Attach(entity);
            entity["test_attribute2"] = "World";
            var result = changeTracker.PerformChanges(entity);

            Assert.Equal(1, result);
            changeTracker.Dispose();
        }

        [Fact]
        public void ChangeTracker_WhenChangedNothingChanged()
        {
            var changeTracker = new ChangeTracker();
            var entity = new Entity("test", Guid.NewGuid());
            entity.Attributes.Add("test_attribute1", "Hello");
            entity.Attributes.Add("test_attribute2", "World");
            var attachFirstTimeResult = changeTracker.Attach(entity);
            var result = changeTracker.PerformChanges(entity);

            Assert.Equal(0, result);
            changeTracker.Dispose();
        }

        [Fact]
        public void ChangeTracker_WhenPerformChangesNonTrackedEntity()
        {
            var changeTracker = new ChangeTracker();
            var id = Guid.NewGuid();
            var entity = new Entity("test", id);
            entity.Attributes.Add("test_attribute1", "Hello");
            entity.Attributes.Add("test_attribute2", "World");
            var attachFirstTimeResult = changeTracker.Attach(entity);
            var result = changeTracker.PerformChanges(entity.ToEntity<Entity>());

            Assert.Equal(2, result);
            changeTracker.Dispose();
        }


        [Fact]
        public void ChangeTracker_WhenDetachSuccessfully()
        {
            var changeTracker = new ChangeTracker();
            var entity = new Entity();
            changeTracker.Attach(entity);
            Assert.Equal(1, changeTracker.TrackedEntities.Count());
            var detachResult = changeTracker.Detach(entity);

            Assert.True(detachResult);
            Assert.Equal(0, changeTracker.TrackedEntities.Count());
            changeTracker.Dispose();
        }

        [Fact]
        public void ChangeTracker_WhenDetachNonTrackedEntity()
        {
            var changeTracker = new ChangeTracker();
            var entity = new Entity();
            changeTracker.Attach(entity);
            Assert.Equal(1, changeTracker.TrackedEntities.Count());
            var detachResult = changeTracker.Detach(new Entity());

            Assert.False(detachResult);
            Assert.Equal(1, changeTracker.TrackedEntities.Count());
            changeTracker.Dispose();
        }
    }
}