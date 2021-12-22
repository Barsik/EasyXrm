using System;
using System.Linq.Expressions;
using EasyXrm.Query;
using EasyXrm.Query.QueryBuilding;
using EasyXrm.Utilities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Extensions
{
    public static class QueryBuilderExtensions
    {
        public static QueryBuilder<TEntity> SetColumns<TEntity>(this QueryBuilder<TEntity> queryBuilder,
            params Expression<Func<TEntity, object>>[] columns) where TEntity : Entity
        {
            queryBuilder.SetColumns(ColumnSet<TEntity>.With(columns));
            return queryBuilder;
        }

        public static QueryBuilder<TEntity> AddFilter<TEntity>(this QueryBuilder<TEntity> queryBuilder,
            LogicalOperator logicalOperator,
            Action<FilterBuilder<TEntity>> filterBuilderOptions) where TEntity : Entity
        {
            Action<FilterBuilder<TEntity>> extendedOptions = (builder) =>
                filterBuilderOptions.Invoke(builder.SetLogicalOperator(logicalOperator));

            queryBuilder.AddFilter(extendedOptions);

            return queryBuilder;
        }


        public static FilterBuilder<TEntity> AddCondition<TEntity>(
            this FilterBuilder<TEntity> filterBuilder,
            Expression<Func<TEntity, object>> attribute, ConditionOperator conditionOperator) where TEntity : Entity
        {
            filterBuilder.AddCondition(ConditionExpression<TEntity>.With(attribute, conditionOperator));

            return filterBuilder;
        }

        public static FilterBuilder<TEntity> AddCondition<TEntity>(
            this FilterBuilder<TEntity> filterBuilder,
            Expression<Func<TEntity, object>> attribute, ConditionOperator conditionOperator, object value)
            where TEntity : Entity
        {
            filterBuilder.AddCondition(ConditionExpression<TEntity>.With(attribute, conditionOperator, value));

            return filterBuilder;
        }
    }
}