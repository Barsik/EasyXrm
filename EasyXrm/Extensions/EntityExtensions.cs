using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EasyXrm.ChangeTracking;
using EasyXrm.Models;
using EasyXrm.Utilities;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Extensions
{
    public static class EntityExtensions
    {
        public static TValue GetAliasedValue<TValue>(this Entity entity, string attributeName, string alias)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var aliasedAttributeName = $"{alias}.{attributeName}";
            var aliasedValue = entity.GetAttributeValue<AliasedValue>(aliasedAttributeName);
            return aliasedValue != null ? (TValue)aliasedValue.Value : default;
        }

        public static TEntity GetAliasedEntity<TEntity>(this Entity entity, string entityAlias)
            where TEntity : Entity, new()
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var aliasedEntity = new TEntity();

            var aliasedAttributes = entity.Attributes
                .Where(a => a.Key.StartsWith($"{entityAlias}.") && a.Value is AliasedValue).Select(a =>
                {
                    var aliasedValue = (AliasedValue)a.Value;
                    return new KeyValuePair<string, object>(aliasedValue.AttributeLogicalName, aliasedValue.Value);
                });


            aliasedEntity.Attributes.AddRange(aliasedAttributes);

            return aliasedEntity;
        }

        public static void Merge<TEntity>(this TEntity source, TEntity mergeWith, bool overwrite = true)
            where TEntity : Entity
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (mergeWith == null) return;

            var guid = source.Id;

            foreach (var attribute in mergeWith.Attributes)
            {
                if (!overwrite && source.Attributes.ContainsKey(attribute.Key))
                {
                    continue;
                }

                source.Attributes[attribute.Key] = attribute.Value;
            }

            if (guid != Guid.Empty) source.Id = guid;
        }

        public static TEntity MergeImmutable<TEntity>(this TEntity source, TEntity mergeWith, bool overwrite = true)
            where TEntity : Entity
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var copy = source.ToEntity<TEntity>();
            Merge(copy, mergeWith, overwrite);
            return copy;
        }

        public static TEntity Diff<TEntity>(this TEntity source, TEntity entity) where TEntity : Entity
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (entity == null)
                return source;

            var diff = source.ToEntity<TEntity>();

            foreach (var attribute in diff.Attributes)
            {
                if (entity.Contains(attribute.Key) &&
                    Equals(entity.Attributes[attribute.Key], diff.Attributes[attribute.Key]))
                {
                    diff.Attributes.Remove(attribute.Key);
                }
            }

            return diff;
        }

        public static ChangeTracker CreateUpdateContext(this Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new ChangeTracker(entity);
        }

        public static string GetFormattedValue(this Entity entity, string attributeName)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            entity.FormattedValues.TryGetValue(attributeName, out var formattedValue);
            return formattedValue;
        }


        public static bool ContainsAll(this Entity entity, params string[] attributes)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            return attributes.All(entity.Contains);
        }

        public static bool ContainsAll<TEntity>(this TEntity entity,
            params Expression<Func<TEntity, object>>[] attributes) where TEntity : Entity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            return LogicalName.GetNames(attributes).All(entity.Contains);
        }

        public static bool ContainsAny(this Entity entity, params string[] attributes)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return attributes.Any(entity.Contains);
        }

        public static bool ContainsAny<TEntity>(this TEntity entity,
            params Expression<Func<TEntity, object>>[] attributes) where TEntity : Entity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return LogicalName.GetNames(attributes).Any(entity.Contains);
        }

        public static bool Contains<TEntity>(this TEntity entity,
            Expression<Func<TEntity, object>> attribute) where TEntity : Entity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return entity.Contains(LogicalName.GetName(attribute));
        }

        public static TValue GetAttributeValue<TValue>(this Entity entity, string attributeName,
            TValue defaultValue = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return entity.Contains(attributeName) ? (TValue)entity[attributeName] : defaultValue;
        }

        public static TEnumeration GetEnumeration<TEnumeration>(this Entity entity, string attributeName,
            TEnumeration defaultValue = default)
            where TEnumeration : Enumeration<TEnumeration>
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return entity.Contains(attributeName)
                ? Enumeration<TEnumeration>.FromValue(
                    ((OptionSetValue)entity[attributeName]).Value)
                : defaultValue;
        }

        public static void SetEnumeration<TEnumeration>(this Entity entity, string attributeName,
            TEnumeration value)
            where TEnumeration : Enumeration<TEnumeration>
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            OptionSetValue optionSetValue = value;

            entity[attributeName] = optionSetValue;
        }


    }
}