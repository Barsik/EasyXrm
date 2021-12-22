using System;
using EasyXrm.Extensions;
using EasyXrm.Utilities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Query.QueryBuilding
{
    public class QueryBuilder
    {
        protected readonly QueryExpression QueryExpression;

        protected QueryBuilder(string entityName)
        {
            QueryExpression = new QueryExpression(entityName)
            {
                PageInfo = new PagingInfo()
                {
                    PageNumber = 1
                },
                ColumnSet = new ColumnSet(false)
            };
        }

        protected QueryBuilder(QueryExpression queryExpression)
        {
            QueryExpression = queryExpression;
        }

        public static QueryBuilder Create(string entityName, Action<QueryExpression> queryExpressionOptions = null)
        {
            var queryBuilder = new QueryBuilder(entityName);
            queryExpressionOptions?.Invoke(queryBuilder.QueryExpression);

            return queryBuilder;
        }

        public static QueryBuilder Create(QueryExpression queryExpression)
        {
            var queryBuilder = new QueryBuilder(queryExpression);

            return queryBuilder;
        }

        public QueryBuilder SetColumns(ColumnSet columnSet)
        {
            QueryExpression.ColumnSet = columnSet;
            return this;
        }

        public QueryBuilder SetPageInfo(Action<PagingInfo> pageInfoOptions)
        {
            pageInfoOptions?.Invoke(QueryExpression.PageInfo);
            return this;
        }

        public QueryBuilder SetFilter(Action<FilterBuilder> filterBuilderOptions)
        {
            var queryFilterBuilder = FilterBuilder.Create();
            filterBuilderOptions?.Invoke(queryFilterBuilder);

            QueryExpression.Criteria = queryFilterBuilder;
            return this;
        }

        public QueryBuilder AddFilter(Action<FilterBuilder> filterBuilderOptions)
        {
            var filterBuilder = FilterBuilder.Create();
            filterBuilderOptions?.Invoke(filterBuilder);

            QueryExpression.Criteria.AddFilter(filterBuilder);
            return this;
        }

        public QueryBuilder AddCondition(ConditionExpression conditionExpression)
        {
            QueryExpression.Criteria.AddCondition(conditionExpression);
            return this;
        }

        public static implicit operator QueryExpression(QueryBuilder queryBuilder)
        {
            return queryBuilder.QueryExpression;
        }
    }

    public class QueryBuilder<TEntity> : QueryBuilder where TEntity : Entity
    {
        protected QueryBuilder() : base(LogicalName.GetName<TEntity>())
        {
        }

        public static QueryBuilder<TEntity> Create(Action<QueryExpression> queryExpressionOptions = null)
        {
            var queryBuilder = new QueryBuilder<TEntity>();
            queryExpressionOptions?.Invoke(queryBuilder.QueryExpression);
            return queryBuilder;
        }

        public QueryBuilder<TEntity> SetColumns(ColumnSet<TEntity> columnSet)
        {
            QueryExpression.ColumnSet = columnSet;
            return this;
        }

        public QueryBuilder<TEntity> AddCondition(ConditionExpression<TEntity> conditionExpression)
        {
            QueryExpression.Criteria.AddCondition(conditionExpression);
            return this;
        }

        public QueryBuilder<TEntity> AddFilter(Action<FilterBuilder<TEntity>> filterBuilderOptions)
        {
            var filterBuilder = FilterBuilder<TEntity>.Create();
            filterBuilderOptions?.Invoke(filterBuilder);
            QueryExpression.Criteria.AddFilter(filterBuilder);
            return this;
        }


        public QueryBuilder<TEntity> SetFilter(Action<FilterBuilder<TEntity>> filterBuilderOptions)
        {
            var filterBuilder = FilterBuilder<TEntity>.Create();
            filterBuilderOptions?.Invoke(filterBuilder);
            QueryExpression.Criteria = filterBuilder;
            return this;
        }
    }
}