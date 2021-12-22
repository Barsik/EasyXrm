using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Query.QueryBuilding
{
    public class FilterBuilder
    {
        protected readonly FilterExpression FilterExpression = new FilterExpression();

        protected FilterBuilder()
        {
        }

        protected FilterBuilder(LogicalOperator filterOperator)
        {
            FilterExpression.FilterOperator = filterOperator;
        }

        public static FilterBuilder Create() => new FilterBuilder();

        public static FilterBuilder Create(LogicalOperator filterOperator) =>
            new FilterBuilder(filterOperator);


        public FilterBuilder AddCondition(ConditionExpression conditionExpression)
        {
            FilterExpression.AddCondition(conditionExpression);
            return this;
        }

        public FilterBuilder SetLogicalOperator(LogicalOperator filterOperator)
        {
            FilterExpression.FilterOperator = filterOperator;
            return this;
        }

        public FilterBuilder AddFilter(Action<FilterBuilder> queryFilterBuilderOptions)
        {
            var queryFilterBuilder = Create();
            queryFilterBuilderOptions?.Invoke(queryFilterBuilder);
            FilterExpression.AddFilter(queryFilterBuilder.FilterExpression);
            return this;
        }

        public static implicit operator FilterExpression(FilterBuilder filterBuilder)
        {
            return filterBuilder.FilterExpression;
        }
    }


    public class FilterBuilder<TEntity> : FilterBuilder where TEntity : Entity
    {
        protected FilterBuilder()
        {
        }

        protected FilterBuilder(LogicalOperator filterOperator) : base(filterOperator)
        {
        }

        public new FilterBuilder<TEntity> SetLogicalOperator(LogicalOperator filterOperator)
        {
            FilterExpression.FilterOperator = filterOperator;
            return this;
        }

        public new static FilterBuilder<TEntity> Create() => new FilterBuilder<TEntity>();

        public new static FilterBuilder<TEntity> Create(LogicalOperator filterOperator) =>
            new FilterBuilder<TEntity>(filterOperator);

        public FilterBuilder<TEntity> AddFilter(Action<FilterBuilder<TEntity>> filterBuilderOptions)
        {
            var queryFilterBuilder = Create();
            filterBuilderOptions?.Invoke(queryFilterBuilder);
            FilterExpression.AddFilter(queryFilterBuilder.FilterExpression);
            return this;
        }


        public FilterBuilder<TEntity> AddCondition(ConditionExpression<TEntity> conditionExpression)
        {
            FilterExpression.AddCondition(conditionExpression);
            return this;
        }
    }
}