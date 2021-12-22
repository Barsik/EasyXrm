using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Extensions
{
    public static class EntityCollectionExtensions
    {
        public static IEnumerable<TOutput> Map<TOutput>(this EntityCollection entityCollection) where TOutput : Entity
        {
            return entityCollection.Map(e => e.ToEntity<TOutput>());
        }

        public static IEnumerable<TOutput> Map<TOutput>(this EntityCollection entityCollection,
            Func<Entity, TOutput> mapper)
        {
            if (entityCollection?.Entities != null && entityCollection.Entities.Count > 0)
            {
                return entityCollection.Entities.Select(mapper);
            }

            return Enumerable.Empty<TOutput>();
        }
    }
}