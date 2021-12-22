using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace EasyXrm.Utilities
{
    public static class LogicalName
    {
        private static int _useCache = 1;
        public static bool UseCache => _useCache == 1;

        private static readonly ConcurrentDictionary<Type, string> EntityLogicalNameCache =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Expression, string> ColumnLogicalNameCache =
            new ConcurrentDictionary<Expression, string>();

        public static void SetUseCache(bool useCache)
        {
            Interlocked.Exchange(ref _useCache, useCache ? 1 : 0);
        }

        public static string GetName<TEntity>(Expression<Func<TEntity, object>> attributeExpression)
            where TEntity : Entity
        {
            if (!UseCache) return GetNameByExpression(attributeExpression);
            if (ColumnLogicalNameCache.TryGetValue(attributeExpression, out var logicalName)) return logicalName;

            logicalName = GetNameByExpression(attributeExpression);
            ColumnLogicalNameCache.TryAdd(attributeExpression, logicalName);

            return logicalName;
        }

        public static string[] GetNames<TEntity>(params Expression<Func<TEntity, object>>[] attributeExpressions)
            where TEntity : Entity
        {
            return attributeExpressions.Select(GetName).ToArray();
        }

        private static string GetNameByExpression<TEntity>(Expression<Func<TEntity, object>> attributeExpression)
            where TEntity : Entity
        {
            string attributeName = null;
            var body = attributeExpression.Body as MemberExpression;

            if (body == null)
            {
                var ubody = (UnaryExpression)attributeExpression.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (body.Member.CustomAttributes != null)
            {
                var customAttributes = body.Member.GetCustomAttributesData();
                var neededAttribute =
                    customAttributes.FirstOrDefault(x =>
                        x.AttributeType == typeof(AttributeLogicalNameAttribute) ||
                        x.AttributeType == typeof(RelationshipSchemaNameAttribute));
                if (neededAttribute != null)
                {
                    attributeName = neededAttribute.ConstructorArguments.FirstOrDefault().Value.ToString();
                }
            }
            else
            {
                attributeName = body.Member.Name;
            }

            return attributeName;
        }


        public static string GetName<TEntity>() where TEntity : Entity
        {
            var entityType = typeof(TEntity);
            if (!UseCache) return GetEntityName(entityType);
            if (EntityLogicalNameCache.TryGetValue(entityType, out var logicalName)) return logicalName;

            logicalName = GetEntityName(entityType);
            EntityLogicalNameCache.TryAdd(entityType, logicalName);

            return logicalName;
        }

        private static string GetEntityName(Type entityType)
        {
            var attribute = entityType.GetCustomAttribute<EntityLogicalNameAttribute>();
            return attribute?.LogicalName;
        }
    }
}