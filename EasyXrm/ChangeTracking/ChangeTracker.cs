using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.ChangeTracking
{
    public class ChangeTracker : IDisposable
    {
        private ConcurrentDictionary<Entity, KeyValuePair<string, object>[]> _tracker =
            new ConcurrentDictionary<Entity, KeyValuePair<string, object>[]>();

        public IEnumerable<Entity> TrackedEntities => _tracker.Keys;

        public ChangeTracker()
        {
        }

        public ChangeTracker(params Entity[] entities)
        {
            Attach(entities);
        }

        public bool Attach(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return _tracker.TryAdd(entity, CloneAttributes(entity));
        }

        public void Attach(Entity[] entities)
        {
            foreach (var entity in entities)
            {
                Attach(entity);
            }
        }

        public KeyValuePair<string, object>[] CloneAttributes(Entity entity)
        {
            var cloneAttributes = new KeyValuePair<string, object>[entity.Attributes.Count];
            entity.Attributes.CopyTo(cloneAttributes, 0);
            return cloneAttributes;
        }

        public void Reattach(Entity entity)
        {
            _tracker[entity] = CloneAttributes(entity);
        }

        public int PerformChanges(Entity entity)
        {
            if (_tracker.ContainsKey(entity))
            {
                var deltaAttributes = _tracker[entity];

                if (deltaAttributes == null) return entity.Attributes.Count;
                var guid = entity.Id;

                foreach (var prev in deltaAttributes)
                {
                    if (entity.Attributes.ContainsKey(prev.Key) && Equals(entity.Attributes[prev.Key], prev.Value))
                    {
                        entity.Attributes.Remove(prev.Key);
                    }
                }

                if (guid != Guid.Empty) entity.Id = guid;
            }

            return entity.Attributes.Count;
        }

        public bool Detach(Entity entity)
        {
            return _tracker.TryRemove(entity, out var _);
        }

        public void PerformChangesForAll()
        {
            foreach (var item in _tracker)
            {
                PerformChanges(item.Key);
            }
        }

        public void Dispose()
        {
            _tracker = null;
        }
    }
}