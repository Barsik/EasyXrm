using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.ChangeTracking
{
    public class ChangeTrackedOrganizationService : IOrganizationService, IDisposable
    {
        private readonly IOrganizationService _organizationService;
        private readonly ChangeTracker _changeTracker;

        public ChangeTracker ChangeTracker => _changeTracker;

        public ChangeTrackedOrganizationService(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
            _changeTracker = new ChangeTracker();
        }

        public ChangeTrackedOrganizationService(IOrganizationService organizationService, ChangeTracker changeTracker)
        {
            _organizationService = organizationService;
            _changeTracker = changeTracker;
        }

        private void CreateTracking(Entity entity)
        {
            _changeTracker.Attach(entity);
        }

        private int UpdateTracking(Entity entity)
        {
            var changes = _changeTracker.PerformChanges(entity);
            _changeTracker.Reattach(entity);
            return changes;
        }

        private void DeleteTracking(string entityName, Guid id)
        {
            var entityToDetach =
                _changeTracker.TrackedEntities.FirstOrDefault(e => e.Id == id && e.LogicalName.Equals(entityName));
            if (entityToDetach != null)
            {
                _changeTracker.Detach(entityToDetach);
            }
        }


        public Guid Create(Entity entity)
        {
            CreateTracking(entity);
            return _organizationService.Create(entity);
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            var entity = _organizationService.Retrieve(entityName, id, columnSet);
            _changeTracker.Attach(entity);
            return entity;
        }

        public void Update(Entity entity)
        {
            if (UpdateTracking(entity) > 0)
            {
                _organizationService.Update(entity);
            }
        }

        public void Delete(string entityName, Guid id)
        {
            DeleteTracking(entityName, id);

            _organizationService.Delete(entityName, id);
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            if (request is CreateRequest createRequest)
            {
                CreateTracking(createRequest.Target);
            }
            else if (request is UpdateRequest updateRequest)
            {
                UpdateTracking(updateRequest.Target);
            }
            else if (request is DeleteRequest deleteRequest)
            {
                DeleteTracking(deleteRequest.Target.LogicalName, deleteRequest.Target.Id);
            }

            return _organizationService.Execute(request);
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship,
            EntityReferenceCollection relatedEntities)
        {
            _organizationService.Associate(entityName, entityId, relationship, relatedEntities);
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship,
            EntityReferenceCollection relatedEntities)
        {
            _organizationService.Disassociate(entityName, entityId, relationship, relatedEntities);
        }

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            var entityCollection = _organizationService.RetrieveMultiple(query);

            foreach (var entity in entityCollection.Entities)
            {
                _changeTracker.Attach(entity);
            }

            return entityCollection;
        }

        public void Dispose()
        {
            _changeTracker?.Dispose();
        }
    }
}