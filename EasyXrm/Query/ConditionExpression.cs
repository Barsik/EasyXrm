using System;
using System.Linq.Expressions;
using EasyXrm.Utilities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Query
{
    public sealed class ConditionExpression<TEntity> where TEntity : Entity
    {
        public string AttributeName { get; }
        public ConditionOperator ConditionOperator { get; }
        public object[] Value { get; }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator)
        {
            AttributeName = attributeName;
            ConditionOperator = conditionOperator;
            Value = Array.Empty<object>();
        }

        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, object value)
        {
            AttributeName = attributeName;
            ConditionOperator = conditionOperator;
            Value = new[] { value };
        }

        public static ConditionExpression<TEntity> With(Expression<Func<TEntity, object>> attribute,
            ConditionOperator conditionOperator, object value)
        {
            return new ConditionExpression<TEntity>(LogicalName.GetName(attribute), conditionOperator, value);
        }

        public static ConditionExpression<TEntity> With(Expression<Func<TEntity, object>> attribute,
            ConditionOperator conditionOperator)
        {
            return new ConditionExpression<TEntity>(LogicalName.GetName(attribute), conditionOperator);
        }

        public static implicit operator ConditionExpression(ConditionExpression<TEntity> conditionExpression)
        {
            return new ConditionExpression(conditionExpression.AttributeName, conditionExpression.ConditionOperator,
                conditionExpression.Value);
        }
    }
}