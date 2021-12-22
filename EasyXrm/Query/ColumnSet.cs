using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EasyXrm.Utilities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Query
{
    public class ColumnSet<TEntity> where TEntity : Entity
    {
        private readonly List<string> _columns;

        public IReadOnlyCollection<string> Columns => _columns.AsReadOnly();

        private ColumnSet(string[] attributes)
        {
            _columns = attributes.ToList();
        }

        public static ColumnSet<TEntity> With(params Expression<Func<TEntity, object>>[] columns)
        {
            var columnSet = new ColumnSet<TEntity>(LogicalName.GetNames(columns));

            return columnSet;
        }


        public void AddColumns(params Expression<Func<TEntity, object>>[] columns)
        {
            _columns.AddRange(LogicalName.GetNames(columns));
        }

        public static implicit operator ColumnSet(ColumnSet<TEntity> columnSet)
        {
            return new ColumnSet(columnSet.Columns.ToArray());
        }
    }
}